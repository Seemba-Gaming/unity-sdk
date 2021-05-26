using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using SeembaSDK.ArabicSupport;
using UnityEngine.UI;

namespace SeembaSDK
{
    public sealed class TranslationManager : MonoBehaviour
    {
        #region Static
        public static TranslationManager _instance { get { return sInstance; } }
        private static TranslationManager sInstance;
        #endregion
        public Font ArabicFont;
        [HideInInspector]
        public string scene = null;
        public static readonly SystemLanguage[] Languages = { SystemLanguage.Arabic, SystemLanguage.English, SystemLanguage.French, SystemLanguage.Spanish, SystemLanguage.German };
        public static bool? isDownloaded = null;
        //public static string systemLanguage = SystemLanguage.Arabic.ToString();
        public static string systemLanguage;
        public static Dictionary<string, string> ShortLanguages = new Dictionary<string, string>();
        private Dictionary<string, Dictionary<string, string>> Translations = new Dictionary<string, Dictionary<string, string>>();

#if UNITY_EDITOR
        private static bool d_OverrideLanguage = true;
        private static SystemLanguage d_Language = SystemLanguage.French;
#endif
        private void Awake()
        {
            sInstance = this;
        }
        private void Start()
        {
            systemLanguage = Application.systemLanguage.ToString();
            ShortLanguages.Add(SystemLanguage.English.ToString(), "en");
            ShortLanguages.Add(SystemLanguage.French.ToString(), "fr");
            ShortLanguages.Add(SystemLanguage.German.ToString(), "de");
            ShortLanguages.Add(SystemLanguage.Arabic.ToString(), "ar");
            ShortLanguages.Add(SystemLanguage.Spanish.ToString(), "es");
        }

        private void CheckInstance()
        {
            if (Translations.Count == 0)
            {
                // Get the current language.
                var lang = Application.systemLanguage;

#if UNITY_EDITOR
                // Override the current language for testing purpose.
                if (d_OverrideLanguage)
                    lang = d_Language;
#endif
                // Check if the current language is supported.
                // Otherwise use the first language as default.
                if (Array.IndexOf<SystemLanguage>(Languages, lang) == -1)
                    lang = Languages[0];


                if (getTranslationFile() != null)
                {
                    ParseFile(getTranslationFile());
                }
            }
        }
        // Returns the translation for this key.
        public string Get(string key, string defaultString = null)
        {
            CheckInstance();
            try
            {
                if (Translations.Count == 0)
                {
                    return null;
                }
                if (systemLanguage.Equals(SystemLanguage.Arabic.ToString()))
                {
                    return ArabicFixer.Fix(Translations[scene][key]);
                }
                else
                {
                    return Translations[scene][key];
                }
            }
            catch (NullReferenceException) { return defaultString; }
        }
        public void ParseFile(string data)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            Translations.Clear();
            foreach (KeyValuePair<string, object> entry in dict)
            {
                Translations.Add(entry.Key, JsonConvert.DeserializeObject<Dictionary<string, string>>(entry.Value.ToString()));
            }
        }
        public async Task<bool> SavePreferedLanguage()
        {
            var url = Endpoint.laguagesURL + "/" + systemLanguage + ".json";
            var lastmodified = await SeembaWebRequest.Get.HttpsLastModifed(url);
            var mCurrentLastModifed = PlayerPrefs.GetString("Last-Modified");
            if (!string.IsNullOrEmpty(mCurrentLastModifed))
            {
                if (mCurrentLastModifed.Equals(lastmodified.ToString()))
                {
                    if (!isLanguageSupported())
                    {
                        await GetUserLanguage(SystemLanguage.English.ToString(), lastmodified);
                        isDownloaded = true;
                        return true;
                    }
                    else
                    {
                        var file = getTranslationFile();
                        if(file != null)
                        {
                            ParseFile(getTranslationFile());
                        }
                        else
                        {
                            return await GetUserLanguage(systemLanguage, lastmodified);
                        }
                        return true;
                    }
                }
                else
                {
                    return await GetUserLanguage(systemLanguage, lastmodified);
                }
            }
            else
            {
                return await GetUserLanguage(systemLanguage, lastmodified);
            }
        }

        public static async Task<bool> GetUserLanguage(string language, DateTime lastmodified)
        {
            var url = Endpoint.laguagesURL + "/" + language + ".json";
            var req = await SeembaWebRequest.Get.HttpsGet(url);

            if (req == null)
            {
                isDownloaded = false;
                return false;
            }
            else
            {
                Debug.LogWarning("here");
                Debug.Log("File Downloaded.");
                string savePath = string.Format("{0}/{1}.json", Application.persistentDataPath, language);
                System.IO.File.WriteAllText(savePath, req);
                PlayerPrefs.SetString(systemLanguage, systemLanguage);
                PlayerPrefs.SetString("Last-Modified", lastmodified.ToString());
                isDownloaded = true;
                return true;
            }
        }


        private static bool isLanguageSupported()
        {
            foreach (SystemLanguage lang in Languages)
            {
                if (lang.ToString().Equals(systemLanguage)) return true;
            }
            return false;
        }


        public static string getTranslationFile()
        {
            return readStringFromFile(systemLanguage + ".json");
        }
        public static string readStringFromFile(string filename)
        {
#if !WEB_BUILD
            string path = pathForDocumentsFile(filename);

            if (File.Exists(path))
            {
                FileStream file = new FileStream(path, System.IO.FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                string str = null;
                str = sr.ReadToEnd();
                sr.Close();
                file.Close();
                return str;
            }
            else
            {

                return null;
            }
#else
			return null;
#endif
        }
        public static string pathForDocumentsFile(string filename)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.Length - 5);
                path = path.Substring(0, path.LastIndexOf('/'));
                if (!Directory.Exists(Path.Combine(path, "Documents")))
                {
                    Directory.CreateDirectory(Path.Combine(path, "Documents"));
                }
                return Path.Combine(Path.Combine(path, "Documents"), filename);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                string path = Application.persistentDataPath;
                return Path.Combine(path, filename);
            }
            else
            {
                string path = Application.persistentDataPath;
                return Path.Combine(path, filename);
            }
        }
    }
}
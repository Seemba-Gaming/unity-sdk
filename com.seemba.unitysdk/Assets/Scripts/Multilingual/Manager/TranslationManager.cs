using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using SimpleJSON;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public sealed class TranslationManager : MonoBehaviour
    {
        public static string scene = null;
        public static readonly SystemLanguage[] Languages = { SystemLanguage.English, SystemLanguage.French, SystemLanguage.Spanish, SystemLanguage.German };
        private static JSONNode Translations = null;
        public static bool? isDownloaded = null;
        //static string systemLanguage = SystemLanguage.English.ToString();
        static string systemLanguage = Application.systemLanguage.ToString();
        public static Dictionary<string, string> ShortLanguages = new Dictionary<string, string>();

#if UNITY_EDITOR
        private static bool d_OverrideLanguage = true;
        private static SystemLanguage d_Language = SystemLanguage.French;
#endif

        private void Start()
        {
            ShortLanguages.Add(SystemLanguage.English.ToString(), "en");
            ShortLanguages.Add(SystemLanguage.French.ToString(), "fr");
            ShortLanguages.Add(SystemLanguage.German.ToString(), "de");
            ShortLanguages.Add(SystemLanguage.Arabic.ToString(), "ar");
            ShortLanguages.Add(SystemLanguage.Spanish.ToString(), "es");
        }

        private static void CheckInstance()
        {
            if (Translations == null)
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
        public static string Get(string key, string defaultString = null)
        {
            CheckInstance();
            try
            {
                return Translations[scene][key].Value;
            }
            catch (NullReferenceException) { return defaultString; }
        }
        public static void ParseFile(string data)
        {
            Translations = JSON.Parse(data);
        }
        public static async Task<bool> SavePreferedLanguage()
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
                        ParseFile(getTranslationFile());
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
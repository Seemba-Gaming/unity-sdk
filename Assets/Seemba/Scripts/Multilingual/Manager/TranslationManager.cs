using SimpleJSON;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
public sealed class TranslationManager : MonoBehaviour
{
    public static string scene = null;
    public static bool? isDownloaded = null;
    public static readonly SystemLanguage[] Languages = { SystemLanguage.English, SystemLanguage.French, SystemLanguage.Spanish, SystemLanguage.German };
    private static JSONNode Translations = null;
    static string systemLanguage = Application.systemLanguage.ToString();
#if UNITY_EDITOR
    private static bool d_OverrideLanguage = false;
    private static SystemLanguage d_Language = SystemLanguage.English;
#endif
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
    public static string Get(string key)
    {
        CheckInstance();
        try
        {


            return Translations[scene][key].Value;
        }
        catch (NullReferenceException ex) { return string.Empty; }
    }
    public static void ParseFile(string data)
    {
        Translations = JSON.Parse(data);
    }
    public static IEnumerator SavePreferedLaguage()
    {
        yield return waitIcon();
        Debug.Log("systemLanguage: " + systemLanguage);
        if (!isLanguageSupported()) { isDownloaded = true; }
        else if ((!systemLanguage.Equals("English")) && (string.IsNullOrEmpty(PlayerPrefs.GetString(systemLanguage))))
        {
            Debug.Log("Downloding " + systemLanguage + " File...");
            string url = Endpoint.laguagesURL + "/" + systemLanguage + ".json";
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                GameObject.Find("downloading").transform.localScale = Vector3.zero;
                GameObject.Find("settingLanguage").transform.localScale = Vector3.one;
                yield return www.Send();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("File cannot Downloaded.");
                    Debug.Log(www.error);
                    isDownloaded = false;
                }
                else
                {
                    Debug.Log("File Downloaded.");
                    Debug.Log(Application.persistentDataPath);
                    string savePath = string.Format("{0}/{1}.json", Application.persistentDataPath, systemLanguage);
                    System.IO.File.WriteAllText(savePath, www.downloadHandler.text);
                    PlayerPrefs.SetString(systemLanguage, systemLanguage);
                    isDownloaded = true;
                }
            }
        }
        else
        {
            isDownloaded = true;
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

    static IEnumerator waitIcon()
    {

        yield return new WaitWhile(() => GamesManager.iconSaved != null);
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
            Debug.Log(path);
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
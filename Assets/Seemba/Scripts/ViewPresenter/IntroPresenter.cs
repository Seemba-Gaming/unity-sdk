using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class IntroPresenter : MonoBehaviour
{
    public Text Title, text;
    string fulltextTitle, fullTextforText;

    // Use this for initialization
    void Start()
    {
        if (!isLaunched())
        {
            saveFirstLaunch();

        }
        StartCoroutine(waitText());
        Debug.Log("fulltextTitle: " + fulltextTitle);
        Debug.Log("fullTextforText: " + fullTextforText);
        Text GameTitle = GameObject.Find("GameTitle").GetComponent<Text>();
        GameTitle.text = GamesManager.GAME_NAME;
        StartCoroutine(WriteText());
    }
    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator WriteText()
    {
        yield return waitText();
        fulltextTitle = IntroTranslationController.CHANGE_THE_WAY_YOU_PLAY;
        fullTextforText = IntroTranslationController.DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA;
        for (int i = 0; i < fulltextTitle.Length; i++)
        {
            Title.text += fulltextTitle.Substring(i, 1);
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < fullTextforText.Length; i++)
        {
            text.text += fullTextforText.Substring(i, 1);
            yield return new WaitForSeconds(0.04f);
        }
    }
    IEnumerator waitText()
    {
        yield return new WaitForSeconds(0.5f);
        //zyield return null;
    }
    public void saveFirstLaunch()
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            writeStringToFile("true", "firstLaunch.dat");
        });
    }
    public bool isLaunched()
    {
        if (readStringFromFile("firstLaunch.dat") == null)
        {
            return false;
        }
        else return true;
    }
    public void writeStringToFile(string str, string filename)
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        ////Debug.Log(path);
        FileStream file = new FileStream(path, System.IO.FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);
        sw.Close();
        file.Close();
#endif
    }
    public string readStringFromFile(string filename)//, int lineIndex )
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, System.IO.FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string str = null;
            str = sr.ReadLine();
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
    public string pathForDocumentsFile(string filename)
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
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
}

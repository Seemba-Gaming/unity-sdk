using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class IntroPresenter : MonoBehaviour
{

    public Text Title;
    public Text MainText;
    public Text GameTitle;
    public Button Continue;
    private string mTitle, mMainText;

    // Use this for initialization
    private void Start()
    {
        GameTitle.text = GamesManager.GAME_NAME;
        //StartCoroutine(waitText());
        StartCoroutine(WriteText());

        Continue.onClick.AddListener(delegate
        {
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Signup.gameObject);
        });
    }

    private IEnumerator WriteText()
    {
        yield return waitText();

        mTitle = IntroTranslationController.CHANGE_THE_WAY_YOU_PLAY;
        mMainText = IntroTranslationController.DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA;
        for (int i = 0; i < mTitle.Length; i++)
        {
            Title.text += mTitle.Substring(i, 1);
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < mMainText.Length; i++)
        {
            MainText.text += mMainText.Substring(i, 1);
            yield return new WaitForSeconds(0.04f);
        }
    }

    private IEnumerator waitText()
    {
        yield return new WaitForSeconds(1f);
    }

    public void writeStringToFile(string str, string filename)
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        FileStream file = new FileStream(path, System.IO.FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);
        sw.Close();
        file.Close();
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

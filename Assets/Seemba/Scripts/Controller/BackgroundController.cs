using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundController : MonoBehaviour
{
    public static Sprite CurrentBackground;
    public static string backgroundURL;
    public Image background;
    // Use this for initialization
    void Start()
    {
        if (CurrentBackground == null)
        {
            background.sprite = LoadBackgroundImage();
        }
        else
        {
            SetBackground();
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public static IEnumerator SaveBackgroundImage(string url)
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("BackgroundURL")) || !PlayerPrefs.GetString("BackgroundURL").Equals(url))
        {
            GameObject.Find("downloading").transform.localScale = Vector3.one;
            PlayerPrefs.SetString("BackgroundURL", url);
            float timer = 0;
            bool failed = false;
            var www = new WWW(url);
            while (!www.isDone)
            {
                if (timer > 25) { failed = true; break; }
                timer += Time.deltaTime;
                yield return null;
            }
            if (failed)
            {
                www.Dispose();
                GamesManager.backgroundSaved = false;
            }
            else
            {
                var texture = www.texture;
                byte[] bytes;
                bytes = texture.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "background.png", bytes);
                GamesManager.backgroundSaved = true;
            }
            if (www.error == null)
            {
                var texture = www.texture;
                byte[] bytes;
                bytes = texture.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "background.png", bytes);
                GamesManager.backgroundSaved = true;
            }
            else
            {
                GamesManager.backgroundSaved = false;
            }
        }
        else
        {
            GamesManager.backgroundSaved = true;
            yield return null;
        }
    }
    static Sprite LoadBackgroundImage()
    {
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + '/' + "background.png");
        Texture2D txt = new Texture2D(1, 1);
        txt.LoadImage(bytes);
        txt.Apply();
        CurrentBackground = Sprite.Create(txt, new Rect(0, 0, txt.width, txt.height), new Vector2(0, 0));
        return CurrentBackground;
    }
    void showBachgroundAnimation()
    {
        GameObject.Find("CanvasMain").GetComponent<Animator>().SetBool("show", true);
    }
    void SetBackground()
    {
        background.sprite = CurrentBackground;
    }
}

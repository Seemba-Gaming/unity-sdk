using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEditor;
using System;

[CLSCompliant(false)]
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
    public async static Task<bool> SaveBackgroundImage(string url)
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("BackgroundURL")) || !PlayerPrefs.GetString("BackgroundURL").Equals(url))
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            await www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                www.Dispose();
                return false;
            }
            else
            {
                PlayerPrefs.SetString("BackgroundURL", url);
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                byte[] bytes;
                bytes = texture.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "background.png", bytes);
                return true;
            }
        }
        else
        {
            return true;
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
    void SetBackground()
    {
        background.sprite = CurrentBackground;
    }
}

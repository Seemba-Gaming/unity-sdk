using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
public class ShareController : MonoBehaviour
{
    private string text = "I challenge you on " + GamesManager.GAME_NAME + ". are you up to the challenge? Can you beat my highscore? Waiting for you on "+GamesManager.GAME_ANDROID_URL;
    public void ShareScreenshot()
    {   
        StartCoroutine(TakeSSAndShare());
    }
    public void ShareText() {
        StartCoroutine(ShareTextInBG());
    }
    private IEnumerator TakeSSAndShare()
    {
        yield return new WaitForEndOfFrame();
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        // To avoid memory leaks
        Destroy(ss);
        Debug.Log(text);
        // new NativeShare().AddFile(filePath).SetText(text).Share();
        // Share on WhatsApp only, if installed (Android only)
        // if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
    }
    private IEnumerator ShareTextInBG()
    {
        yield return new WaitForEndOfFrame();
        // new NativeShare().SetText(text).Share();
        Debug.Log(text);
    }
}

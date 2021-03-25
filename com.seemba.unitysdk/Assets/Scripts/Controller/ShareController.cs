using System.Collections;
using UnityEngine;
using System.IO;
using System;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class ShareController : MonoBehaviour
    {
        private string text;
        public void ShareScreenshot()
        {
            StartCoroutine(TakeSSAndShare());
        }
        public void ShareText()
        {
            StartCoroutine(ShareTextInBG());
        }
        private IEnumerator TakeSSAndShare()
        {

            yield return new WaitForEndOfFrame();
            TranslationManager.scene = "Sharing";
            text = TranslationManager.Get("i_challenge_you_on") + " "  + GamesManager.GAME_NAME + ". " + TranslationManager.Get("are_you_up") + GamesManager.GAME_ANDROID_URL + " \n" + GamesManager.GAME_IOS_URL;
            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();
            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, ss.EncodeToPNG());
            // To avoid memory leaks
            Destroy(ss);
            Debug.Log(text);
            new NativeShare().AddFile(filePath).SetText(text).Share();
            // Share on WhatsApp only, if installed (Android only)
            //if( NativeShare.TargetExists( "com.whatsapp" ) )
            //	new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
        }
        private IEnumerator ShareTextInBG()
        {
            yield return new WaitForEndOfFrame();
            if(GamesManager.GAME_ANDROID_URL != null && GamesManager.GAME_IOS_URL != null)
            {
                if(Application.platform == RuntimePlatform.Android)
                {
                    text = GamesManager.GAME_ANDROID_URL;
                }
                else
                {
                    text = GamesManager.GAME_IOS_URL;
                }
            }
            else if (GamesManager.GAME_ANDROID_URL == null)
            {
                    text = GamesManager.GAME_IOS_URL;
            }
            else
            {
                text = GamesManager.GAME_ANDROID_URL;
            }
            new NativeShare().SetText(text).Share();
        }
    }
}

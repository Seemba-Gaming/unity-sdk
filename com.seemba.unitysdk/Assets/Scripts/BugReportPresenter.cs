using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class BugReportPresenter : MonoBehaviour
    {
        public Button ReportBug;
        public InputField BugDescription;
        public Image ScreenShot;
        public Animator Animator;

        public void Init(Sprite screenShot)
        {
            ScreenShot.sprite = screenShot;
        }

        public async void SendScreenShot()
        {
            Debug.LogWarning("Send Bug");
            await SendBugReport(ScreenShot, GamesManager.GAME_ID, BugDescription.text);
        }
        public async Task<bool> SendBugReport(Image screenshot, string game_id, string description)
        {
            WWWForm form = new WWWForm();
            Texture2D mytexture = screenshot.sprite.texture;
            byte[] bytes;
            bytes = mytexture.EncodeToPNG();
            var screedShotUrl = await ImagesManager.FixImage(bytes);
            form.AddField("screenshot", screedShotUrl);
            form.AddField("game_id", game_id);
            form.AddField("description", description);
            string url = Endpoint.classesURL + "/games/report";
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            return true;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SeembaSDK
{
    [Serializable]
    public class SeembaResponse<T>
    {
        public bool success;
        public string message;
        public T data;
        public SeembaResponse(bool success, string message, T data, string token)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }
    }

    public class SeembaWebRequest : MonoBehaviour
    {
        #region Static
        public static SeembaWebRequest Get { get { return sInstance; } }

        private static SeembaWebRequest sInstance;
        #endregion
        //
        // Résumé :
        //     Create a UnityWebRequest for HTTP GET.
        //
        // Paramètres :
        //   uri:
        //     The URI of the resource to retrieve via HTTP GET.
        //
        // Retourne :
        //     An object that retrieves data from the uri.
        public delegate void StringResponseEventHandler(UnityWebRequest data);
        public delegate void TextureResponseEventHandler(int id, Texture2D data);
        public event StringResponseEventHandler OnSeembaErrorEvent;

        private void Start()
        {
            sInstance = this;
        }
        public async Task<string> HttpsGet(string uri)
        {
            UnityWebRequest www = UnityWebRequest.Get(uri);
            var token = UserManager.Get.getCurrentSessionToken();
            if (token != null)
            {
                www.SetRequestHeader("x-access-token", token);
                www.SetRequestHeader("Sdk-Version", Endpoint.SdkVersion);
            }
            return await HandleRequest(www);
        }

        public async Task<T> HttpsGetJSON<T>(string uri)
        {
            string responseText = await HttpsGet(uri);
            SeembaResponse<T> response = JsonConvert.DeserializeObject<SeembaResponse<T>>(responseText);
            return response.data;
        }

        public async Task<string> HttpsPostBearer(string uri, WWWForm postData, string token = null)
        {
            UnityWebRequest www = UnityWebRequest.Post(uri, postData);
            if (string.IsNullOrEmpty(token))
            {
                var userToken = UserManager.Get.getCurrentSessionToken();
                if (userToken != null)
                {
                    www.SetRequestHeader("Authorization", "Bearer " + userToken);
                    www.SetRequestHeader("Sdk-Version", Endpoint.SdkVersion);
                    www.uploadHandler.contentType = "application/x-www-form-urlencoded";
                }
            }
            else
            {
                www.SetRequestHeader("Authorization", "Bearer " + token);
                www.uploadHandler.contentType = "application/x-www-form-urlencoded";
                www.SetRequestHeader("Sdk-Version", Endpoint.SdkVersion);
            }
            return await HandleRequest(www);
        }

        public async Task<string> HttpsPost(string uri, WWWForm postData)
        {
            UnityWebRequest www = UnityWebRequest.Post(uri, postData);
            var token = UserManager.Get.getCurrentSessionToken();
            if (token != null)
            {
                www.SetRequestHeader("x-access-token", token);
                www.SetRequestHeader("content-type", "application/x-www-form-urlencoded");
                www.SetRequestHeader("Sdk-Version", Endpoint.SdkVersion);
            }
            return await HandleRequest(www);
        }


        public async Task<string> HttpsPut(string uri, byte[] bodyData)
        {
            UnityWebRequest www = UnityWebRequest.Put(uri, bodyData);
            var token = UserManager.Get.getCurrentSessionToken();
            if (token != null)
            {
                www.SetRequestHeader("x-access-token", token);
                www.SetRequestHeader("content-type", "application/x-www-form-urlencoded");
                www.SetRequestHeader("Sdk-Version", Endpoint.SdkVersion);
            }
            return await HandleRequest(www);
        }

        public async Task<DateTime> HttpsLastModifed(string uri)
        {
            UnityWebRequest www = UnityWebRequest.Get(uri);
            await www.SendWebRequest();
            return DateTime.Parse(www.GetResponseHeader("Last-Modified"));
        }

        private async Task<string> HandleRequest(UnityWebRequest www)
        {
            await www.SendWebRequest();

            if (www.error != null)
            {
                OnSeembaError(www);
            }
            return www.downloadHandler.text;
        }

        void OnSeembaError(UnityWebRequest data)
        {
            OnSeembaErrorEvent(data);
        }
    }
}
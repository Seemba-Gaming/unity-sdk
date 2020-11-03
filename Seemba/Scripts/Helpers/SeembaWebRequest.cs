using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
class SeembaResponse<T>
{
    public bool success;
    public string message;
    public T data;

    public SeembaResponse(bool success, string message, T data)
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
    public delegate void StringResponseEventHandler(string data);
    public delegate void TextureResponseEventHandler(int id, Texture2D data);
    public event StringResponseEventHandler OnSeembaErrorEvent;

    private void Start()
    {
        sInstance = this;
    }
    public async System.Threading.Tasks.Task<string> HttpsGet(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        var token = UserManager.Get.getCurrentSessionToken();
        if(token != null)
        {
            www.SetRequestHeader("x-access-token",token );
        }
        return await HandleRequest(www);
    }
    
    public async System.Threading.Tasks.Task<string> HttpsGetAnonymous(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);

        return await HandleRequest(www);
    }

    public async System.Threading.Tasks.Task<T> HttpsGetJSON<T>(string uri)
    {
        string responseText = await HttpsGet(uri);
        Debug.LogWarning(responseText);
        SeembaResponse<T> response = JsonConvert.DeserializeObject<SeembaResponse<T>>(responseText);
        return response.data;
    }

    private async System.Threading.Tasks.Task<string> HandleRequest(UnityWebRequest www)
    {
        await www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) 
        { 
            OnSeembaError(www.downloadHandler.text);
            return null;
        } 

        return www.downloadHandler.text;
    }

    void OnSeembaError(string data)
    {
        OnSeembaErrorEvent(data);
    }
}
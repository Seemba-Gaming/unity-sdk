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
    public static async System.Threading.Tasks.Task<string> Get(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("x-access-token", UserManager.Get.getCurrentSessionToken());

        return await handleRequest(www);
    }
    
    public static async System.Threading.Tasks.Task<string> GetAnonymous(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);

        return await handleRequest(www);
    }

    public static async System.Threading.Tasks.Task<T> GetJSON<T>(string uri)
    {
        string data = await Get(uri);
        SeembaResponse<T> challengeData = JsonConvert.DeserializeObject<SeembaResponse<T>>(data);
        return challengeData.data;
    }

    private static async System.Threading.Tasks.Task<string> handleRequest(UnityWebRequest www)
    {
        await www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) { } //EVENT;

        return www.downloadHandler.text;
    }
}
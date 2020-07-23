﻿using SimpleJSON;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
public class TournamentManager
{
    public const int TOURNAMENT_8 = 8;
    public const int TOURNAMENT_16 = 16;
    public const int TOURNAMENT_32 = 32;
    public const float FEE_BRACKET_CASH_CONFIDENT = 2.00f;
    public const float FEE_BRACKET_CASH_CHAMPION = 4.00f;
    public const float FEE_BRACKET_CASH_LEGEND = 8.00f;
    public const float WIN_BRACKET_CASH_CONFIDENT = 8.00f;
    public const float WIN_BRACKET_CASH_CHAMPION = 16.00f;
    public const float WIN_BRACKET_CASH_LEGEND = 32.00f;
    public const float FEE_BRACKET_BUBBLE_CONFIDENT = 5.00f;
    public const float FEE_BRACKET_BUBBLE_CHAMPION = 10.00f;
    public const float FEE_BRACKET_BUBBLE_LEGEND = 20.00f;
    public const float WIN_BRACKET_BUBBLE_CONFIDENT = 25f;
    public const float WIN_BRACKET_BUBBLE_CHAMPION = 50f;
    public const float WIN_BRACKET_BUBBLE_LEGEND = 100f;
    public const string GAIN_TYPE_CASH = "cash";
    public const string GAIN_TYPE_BUBBLE = "bubble";
    public JSONArray getUserPendingTournaments(string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/tournaments/pending/" + GamesManager.GAME_ID;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();

                    var json = JSON.Parse(jsonResponse);
                    return json["data"].AsArray;
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log(ex);
            return null;
        }
    }
    public JSONArray getUserFinishedTournaments(string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/tournaments/finished/" + GamesManager.GAME_ID;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();

                    var json = JSON.Parse(jsonResponse);
                    return json["data"].AsArray;
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log(ex);
            return null;
        }
    }
    public JSONNode getTournament(string id, string token)
    {
        string url = Endpoint.classesURL + "/tournaments/" + id;
        Debug.Log("Tournament ID:" + id);
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        Debug.Log(token);
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var json = JSON.Parse(jsonResponse);
                    return json;
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log(ex);
            return null;
        }
    }
    public string addScoreInTournament(string tournamentsID, int score, string user_id, string token)
    {
        string url = Endpoint.classesURL + "/tournaments/" + tournamentsID;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.Timeout = 5000;
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (var stream = request.GetRequestStream())
            {
                string json = "score=" + score + "&user_id=" + user_id;
                byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
                stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
            }
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    return N["tournament"]["_id"].Value;
                }
            }
        }
        catch (WebException ex)
        {
            //Debug.Log(ex);
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        //Debug.Log(error);
                        var N = JSON.Parse(error);
                        if (N["success"].AsBool == false)
                        {
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            else
                return null;
        }
    }
    public string JoinOrCreateTournament(int nb_player, float gain, string gain_type, string userId, string token)
    {
        string url = Endpoint.classesURL + "/tournaments";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.Timeout = 5000;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (var stream = request.GetRequestStream())
            {
                string json = "nb_player=" + nb_player + "&gain=" + gain + "&gain_type=" + gain_type + "&game_id=" + GamesManager.GAME_ID + "&user_id=" + userId;
                byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
                stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
            }
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    UserManager.updateUserCredit((N["user"]["money_credit"].AsFloat).ToString(), N["user"]["bubble_credit"].Value);
                    return N["tournament"]["_id"].Value;
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        var N = JSON.Parse(error);
                        if (N["success"].AsBool == false)
                        {
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            else
                return null;
        }
    }
    public bool MyRemoteCertificateValidationCallback(System.Object sender,
        X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain,
        // look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    continue;
                }
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build((X509Certificate2)certificate);
                if (!chainIsValid)
                {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }
}

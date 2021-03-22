using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class TournamentManager : MonoBehaviour
    {
        #region Static
        public static TournamentManager Get { get { return sInstance; } }
        private static TournamentManager sInstance;

        public static List<string> AVALAIBLE_TOURNAMENTS = new List<string>();
        public const int TOURNAMENT_8 = 8;
        public const int TOURNAMENT_16 = 16;
        public const int TOURNAMENT_32 = 32;
        public static float FEE_BRACKET_CASH_AMATEUR = 2.00f;
        public static float FEE_BRACKET_CASH_NOVICE = 4.00f;
        public static float FEE_BRACKET_CASH_CONFIRMED = 8.00f;
        public static float WIN_BRACKET_CASH_AMATEUR = 8.00f;
        public static float WIN_BRACKET_CASH_NOVICE = 16.00f;
        public static float WIN_BRACKET_CASH_CONFIRMED = 32.00f;
        public static float FEE_BRACKET_BUBBLE_AMATEUR = 5.00f;
        public static float FEE_BRACKET_BUBBLE_NOVICE = 10.00f;
        public static float FEE_BRACKET_BUBBLE_CONFIRMED = 20.00f;
        public static float WIN_BRACKET_BUBBLE_AMATEUR = 25f;
        public static float WIN_BRACKET_BUBBLE_NOVICE = 50f;
        public static float WIN_BRACKET_BUBBLE_CONFIRMED = 100f;
        public const string GAIN_TYPE_CASH = "cash";
        public const string GAIN_TYPE_BUBBLE = "bubble";
        public const string BRACKET_TYPE_CONFIDENT = "Confident";
        public const string BRACKET_TYPE_CHAMPION = "Legend";
        public const string BRACKET_TYPE_LEGEND = "Champion";
        #endregion

        #region Unity Methods
        private void Awake()
        {
            sInstance = this;
        }
        #endregion

        #region Methods
        public void InitFees(GameChallengesInfo fees)
        {
            FEE_BRACKET_CASH_AMATEUR = (int)fees.tournament.Amateur.cash * 100;
            FEE_BRACKET_CASH_NOVICE = (int)fees.tournament.Novice.cash * 100;
            FEE_BRACKET_CASH_CONFIRMED = (int)fees.tournament.Confirmed.cash * 100;
            FEE_BRACKET_BUBBLE_AMATEUR = fees.tournament.Amateur.bubbles;
            FEE_BRACKET_BUBBLE_NOVICE = fees.tournament.Novice.bubbles;
            WIN_BRACKET_BUBBLE_CONFIRMED = fees.tournament.Confirmed.bubbles;
        }

        public void InitGains(GameChallengesInfo gain)
        {

            WIN_BRACKET_CASH_AMATEUR = (int)gain.tournament.Amateur.cash * 100;
            WIN_BRACKET_CASH_NOVICE = (int)gain.tournament.Novice.cash * 100;
            WIN_BRACKET_CASH_CONFIRMED = (int)gain.tournament.Confirmed.cash * 100;
            WIN_BRACKET_BUBBLE_AMATEUR = gain.tournament.Amateur.bubbles;
            WIN_BRACKET_BUBBLE_NOVICE = gain.tournament.Novice.bubbles;
            WIN_BRACKET_BUBBLE_CONFIRMED = gain.tournament.Confirmed.bubbles;
        }

        public float GetChallengeFee(float gain, string gainType)
        {
            if (gainType.Equals(GAIN_TYPE_CASH))
            {
                if (gain.Equals(WIN_BRACKET_CASH_AMATEUR))
                {
                    return FEE_BRACKET_CASH_AMATEUR;
                }
                else if (gain.Equals(WIN_BRACKET_CASH_NOVICE))
                {
                    return FEE_BRACKET_CASH_NOVICE;
                }
                else
                {
                    return FEE_BRACKET_CASH_CONFIRMED;
                }
            }
            else
            {
                if (gain.Equals(WIN_BRACKET_BUBBLE_AMATEUR))
                {
                    return FEE_BRACKET_BUBBLE_AMATEUR;
                }
                else if (gain.Equals(WIN_BRACKET_BUBBLE_NOVICE))
                {
                    return FEE_BRACKET_BUBBLE_NOVICE;
                }
                else
                {
                    return WIN_BRACKET_BUBBLE_CONFIRMED;
                }
            }
        }
        public async Task<JSONArray> getUserPendingTournaments(string token)
        {
            string url = Endpoint.classesURL + "/tournaments/pending/" + GamesManager.GAME_ID;
            var json = JSON.Parse(await SeembaWebRequest.Get.HttpsGet(url));
            return json["data"].AsArray;
        }
        public async Task<JSONArray> getUserFinishedTournaments()
        {

            string url = Endpoint.classesURL + "/tournaments/finished/" + GamesManager.GAME_ID;
            var req = await SeembaWebRequest.Get.HttpsGet(url);
            var json = JSON.Parse(req);
            return json["data"].AsArray;
        }
        public async Task<JSONNode> getTournament(string id, string token)
        {
            string url = Endpoint.classesURL + "/tournaments/" + id;
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            var response = await SeembaWebRequest.Get.HttpsGet(url);
            var json = JSON.Parse(response);
            return json;
        }
        public async Task<bool> addScore(string tournamentsID, float score)
        {

            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            string json = "score=" + score + "&user_id=" + UserManager.Get.getCurrentUserId();
            byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);

            string url = Endpoint.classesURL + "/tournaments/" + tournamentsID;
            var response = await SeembaWebRequest.Get.HttpsPut(url, jsonAsBytes);
            if (!string.IsNullOrEmpty(response))
            {
                return true;
            }
            return false;
        }
        public async Task<string> JoinOrCreateTournament(int nb_player, float gain, string gain_type, string userId, string token)
        {
            string url = Endpoint.classesURL + "/tournaments";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            WWWForm form = new WWWForm();
            form.AddField("nb_player", nb_player);
            form.AddField("gain", gain.ToString());
            form.AddField("gain_type", gain_type);
            form.AddField("game_id", GamesManager.GAME_ID);
            form.AddField("user_id", userId);
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            var tournementdata = JSON.Parse(response);
            UserManager.Get.UpdateUserCredit((tournementdata["user"]["money_credit"].AsFloat).ToString(), tournementdata["user"]["bubble_credit"].Value);
            TournamentController.setCurrentTournamentID(tournementdata["tournament"]["_id"].Value);
            return tournementdata["tournament"]["_id"].Value;
        }
        public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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
        #endregion
    }
}

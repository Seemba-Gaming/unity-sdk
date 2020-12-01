
using UnityEngine;
using System.Net;
using SimpleJSON;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System;
namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class ChargeManager : MonoBehaviour
    {


        #region Static
        public static ChargeManager Get { get { return sInstance; } }

        private static ChargeManager sInstance;
        #endregion

        public const string PAYMENT_TYPE = "card";
        public const string PAYMENT_STATUS_REQUIRES_SOURCE_ACTION = "requires_source_action";
        public const string PAYMENT_STATUS_REQUIRES_PAYMENT_METHOD = "requires_payment_method";
        public const string PAYMENT_STATUS_REQUIRES_ACTION = "requires_action";
        public const string PAYMENT_STATUS_SUCCEEDED = "succeeded";
        public const string PAYMENT_STATUS_CANCELED = "canceled";
        public const string PAYMENT_STATUS_PROCESSING = "processing";
        public const string PAYMENT_NEXT_ACTION_TYPE_USE_STRIPE_SDK = "use_stripe_sdk";
        public const string PAYMENT_NEXT_ACTION_TYPE_REDIRECT_TO_URL = "redirect_to_url";
        private void Awake()
        {
            sInstance = this;
        }
        public async System.Threading.Tasks.Task<string> CreatePaymentMethodAsync(string card_number, string cvc, int card_expiry_month, int card_expiry_year)
        {
            string url = Endpoint.stripeURL + "/payment_methods";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            WWWForm form = new WWWForm();
            form.AddField("card[number]", card_number);
            form.AddField("card[exp_month]", card_expiry_month);
            form.AddField("card[exp_year]", card_expiry_year);
            form.AddField("card[cvc]", cvc);
            form.AddField("type", PAYMENT_TYPE);
            var response = await SeembaWebRequest.Get.HttpsPostBearer(url, form, Endpoint.TokenizationAccount);
            var res = JSON.Parse(response);
            return res["id"].Value;
        }
        public async System.Threading.Tasks.Task<JSONNode> CreatePaymentIntentAsync(string _paymentMethod, float amount, string token)
        {
            string url = Endpoint.classesURL + "/payments/charge/";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            WWWForm form = new WWWForm();
            form.AddField("payment_method", _paymentMethod);
            form.AddField("amount", (amount * 100).ToString());
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            Debug.LogWarning(response);
            return JSON.Parse(response);
        }
        public async System.Threading.Tasks.Task<string> isChargeConfirmedAsync(string _paymentIntent, string token)
        {
            string url = Endpoint.classesURL + "/payments/charge/" + _paymentIntent;
            var response = await SeembaWebRequest.Get.HttpsGet(url);
            Debug.Log(response);
            var N = JSON.Parse(response);
            Debug.Log("status: " + N["data"]["status"].Value);
            return N["data"]["status"].Value;
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
}

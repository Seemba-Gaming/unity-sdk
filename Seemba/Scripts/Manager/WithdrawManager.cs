using System.Net;
using System;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeembaSDK
{
    public class WithdrawManager
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        public const string DOC_TYPE_PASSPORT = "passport";
        public const string DOC_TYPE_ID_PROOF = "id_proof";
        public const string WITHDRAW_FAILED_TITLE_DELAY = "Delay";
        public const string WITHDRAW_FAILED_MESSAGE_DELAY = "You have to wait 14 days after you credited your account to be able to withdraw your funds";
        public const string WITHDRAW_FAILED_MESSAGE = "Sorry, Operation Has Failed, Please Try Again Later";
        public const string WITHDRAW_INSUFFICIENT_FUNDS_FAILED_MESSAGE = "Insufficient funds in your account";
        public const string WITHDRAW_INSUFFICIENT_AMOUNT_FAILED_MESSAGE = "Amount should be more than 100€";
        public const string ACCOUNT_VERIFICATION_STATUS_VERIFIED = "verified";
        public const string ACCOUNT_VERIFICATION_STATUS_UNVERIFIED = "unverified";
        public const string ACCOUNT_VERIFICATION_STATUS_PENDING = "pending";
        public const string WITHDRAW_SUCCEEDED_STATUS = "succeeded";
        public const string WITHDRAW_ERROR_BALANCE_INSUFFICIENT = "balance_insufficient";
        public const string WITHDRAW_ERROR_AMOUNT_INSUFFICIENT = "amount_insufficient";
        public const string WITHDRAW_BUSINESS_PROFILE_URL = "www.seemba.com";

        public async Task<string> TokenizeAccountAsync()
        {
            string url = Endpoint.stripeURL + "/tokens";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WWWForm form = new WWWForm();
            form.AddField("account[tos_shown_and_accepted]", "true");
            form.AddField("account[business_type]", "individual");
            var response = await SeembaWebRequest.Get.HttpsPostBearer(url, form, Endpoint.TokenizationAccount);
            var N = JSON.Parse(response);
            return N["id"].Value;
        }
        public async Task<string> TokenizeBankAccount(string country_code, string currency, string iban)
        {
            string url = Endpoint.stripeURL + "/tokens";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WWWForm form = new WWWForm();
            form.AddField("bank_account[country]", country_code);
            form.AddField("bank_account[currency]", currency);
            form.AddField("bank_account[account_number]", iban);
            var response = await SeembaWebRequest.Get.HttpsPostBearer(url, form, Endpoint.TokenizationAccount);
            var N = JSON.Parse(response);
            return N["id"].Value;
        }
        public async Task<bool> CreateConnectAccount(string account_token, string bank_account_token, string currency, string country_code, string token)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            string url = Endpoint.classesURL + "/payments/create/connect_account";
            WWWForm form = new WWWForm();
            form.AddField("ct", account_token);
            form.AddField("external_account", bank_account_token);
            form.AddField("currency", currency);
            form.AddField("country_code", country_code);
            Debug.LogWarning("ct " + account_token);
            Debug.LogWarning("external_account " + bank_account_token);
            Debug.LogWarning("currency " + currency);
            Debug.LogWarning("country_code " + country_code);
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            Debug.LogWarning("CreateConnectAccount " + response);
            var N = JSON.Parse(response);
            return N["success"].AsBool;
        }

        public async Task<bool> attachTokenToAccountAsync(string account_token, string token)
        {
            string url = Endpoint.classesURL + "/payments/updateAccount";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            WWWForm form = new WWWForm();
            form.AddField("external_account", account_token);
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            Debug.Log(response);
            var N = JSON.Parse(response);
            return N["success"].AsBool;
        }

        public async Task<AccountStatus> accountVerificationStatus(string token)
        {
            string url = Endpoint.classesURL + "/payments/retreiveAccount/";
            var response = await SeembaWebRequest.Get.HttpsGet(url);
            AccountStatus accountStatus = JsonConvert.DeserializeObject<AccountStatus>(response);
            return accountStatus;
        }
        public async Task<JSONNode> accountVerificationJSON(string token)
        {
            string url = Endpoint.classesURL + "/payments/retreiveAccount/";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            var response = await SeembaWebRequest.Get.HttpsGet(url);
            var N = JSON.Parse(response);
            return N;
        }
        public bool validateIBAN(string bankAccount)
        {
            try
            {
                bankAccount = bankAccount.ToUpper(); //IN ORDER TO COPE WITH THE REGEX BELOW
                if (String.IsNullOrEmpty(bankAccount))
                    return false;
                else if (System.Text.RegularExpressions.Regex.IsMatch(bankAccount, "^[A-Z0-9]"))
                {
                    bankAccount = bankAccount.Replace(" ", String.Empty);
                    string bank =
                    bankAccount.Substring(4, bankAccount.Length - 4) + bankAccount.Substring(0, 4);
                    int asciiShift = 55;
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in bank)
                    {
                        int v;
                        if (Char.IsLetter(c)) v = c - asciiShift;
                        else v = int.Parse(c.ToString());
                        sb.Append(v);
                    }
                    string checkSumString = sb.ToString();
                    int checksum = int.Parse(checkSumString.Substring(0, 1));
                    for (int i = 1; i < checkSumString.Length; i++)
                    {
                        int v = int.Parse(checkSumString.Substring(i, 1));
                        checksum *= 10;
                        checksum += v;
                        checksum %= 97;
                    }
                    return checksum == 1;
                }
                else
                    return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        public async Task<string> Payout(string token, float amount)
        {
            string url = Endpoint.classesURL + "/payments/payout/";
            WWWForm form = new WWWForm();
            form.AddField("amount", amount.ToString());
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            var N = JSON.Parse(response);
            if (N["success"].AsBool == true)
            {
                return WITHDRAW_SUCCEEDED_STATUS;
            }
            else
            {
                return (!string.IsNullOrEmpty(N["error"]["code"])) ? N["error"]["code"].Value : "error";
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
}

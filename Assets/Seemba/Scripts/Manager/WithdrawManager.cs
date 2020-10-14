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
using System.Security.Policy;

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
    public string uploadImage(string fullPath)
    {
        UserManager um = new UserManager();
        string url = Endpoint.cloudURL + "uploadImage";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["X-Parse-Application-Id"] = "seembaapi";
        request.Headers["Access-Control-Request-Headers"] = "Content-Type";
        request.Headers["Access-Control-Request-Headers"] = "Authorization";
        request.ContentType = @"application/json";
        using (var stream = request.GetRequestStream())
        {
            //Debug.Log (Convert.ToBase64String (File.ReadAllBytes (fullPath)));
            byte[] jsonAsBytes = encoding.GetBytes("{\"bytes\":\"" + Convert.ToBase64String(File.ReadAllBytes(fullPath)) + "\"}");
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    //Debug.Log (jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    return N["result"].Value;
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
                        //Debug.Log (error);
                    }
                }
            }
            return null;
        }
    }

    public async Task<string> TokenizeAccount()
    {
        string url = Endpoint.stripeURL + "/tokens";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        WWWForm form = new WWWForm();
        form.AddField("account[tos_shown_and_accepted]", "true");
        form.AddField("account[business_type]", "individual");
        var www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("Authorization", "Bearer " + Endpoint.TokenizationAccount);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";

        await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) return null;

        var N = JSON.Parse(www.downloadHandler.text);
        Debug.Log(www.downloadHandler.text);
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
        var www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("Authorization", "Bearer " + Endpoint.TokenizationAccount);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";

        await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) { Debug.Log(www.error); return null; }

        var N = JSON.Parse(www.downloadHandler.text);
        Debug.Log(www.downloadHandler.text);
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
        var www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("x-access-token", token);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";

        await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) return false;
        Debug.Log(www.downloadHandler.text);
        var N = JSON.Parse(www.downloadHandler.text);
        return N["success"].AsBool;
    }
    public string HttpUploadFile(string path, string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/upload/doc/";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "bytes=" + Convert.ToBase64String(File.ReadAllBytes(path));
            byte[] jsonAsBytes = encoding.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    var N = JSON.Parse(jsonResponse);
                    if (N["success"].AsBool)
                    {
                        PlayerPrefs.SetString("DocId", N["data"]["id"].Value);
                        return N["data"]["id"].Value;
                    }
                    else
                    {
                        return null;
                    }
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
                    }
                }
            }
            return "error";
        }
    }
    public async Task<bool> attachInfoToAccount(string token, string value, params string[] Params)
    {
        string fieldname = "individual";
        foreach (string param in Params)
        {
            fieldname += "[" + param + "]";
        }
        Debug.Log("Fieldname: " + fieldname);
        WWWForm form = new WWWForm();
        form.AddField(fieldname, value);
        UnityWebRequest request = UnityWebRequest.Post(Endpoint.classesURL + "/payments/updateAccount", form);
        request.SetRequestHeader("x-access-token", token);
        request.SetRequestHeader("content-type", "application/x-www-form-urlencoded");
        await request.SendWebRequest();
        if (request.isNetworkError)
        {
            return false;
        }
        if (request.isHttpError) // Error 
        {
            Debug.Log(request.error);
            return false;
        }
        else // Success
        {
            Debug.Log(request.downloadHandler.text);
            var N = JSON.Parse(request.downloadHandler.text);
            Debug.Log("Success: " + N["success"]);
            return N["success"].AsBool;
        }
    }
    public bool attachInfoToAccount1(string token, string value, params string[] Params)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/updateAccount";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            Debug.Log("value: " + value);
            string json = "individual";
            foreach (string param in Params)
            {
                json += "[" + param + "]";
            }
            json += "=" + value;
            Debug.Log(json);
            byte[] jsonAsBytes = encoding.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
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
                    var N = JSON.Parse(jsonResponse);
                    return N["success"].AsBool;
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
                    }
                }
            }
            return false;
        }
    }
    public bool attachDOBToAccount(string token, int day, int month, int year)
    {
        Debug.Log("attachDOBToAccount:  " + "day: " + day + " month: " + month + " year: " + year);
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/updateAccount";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "";
            json = "individual[dob][day]=" + day + "&&individual[dob][month]=" + month + "&&individual[dob][year]=" + year;
            byte[] jsonAsBytes = encoding.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log("attachDOBToAccount: " + jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    return N["success"].AsBool;
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
                        Debug.Log("WebException eroor: " + error);
                    }
                }
            }
            Debug.Log("WebException DOB");
            return false;
        }
    }
    
    public bool attachDocToAccount(string document_type, string file, string file_side, string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/updateAccount";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "individual[verification][" + document_type + "][" + file_side + "]=" + file;
            byte[] jsonAsBytes = encoding.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
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
                    var N = JSON.Parse(jsonResponse);
                    return N["success"].AsBool;
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
                    }
                }
            }
            return false;
        }
    }

    public async Task<JSONNode> accountVerificationStatus(string token)
    {
        string url = Endpoint.classesURL + "/payments/retreiveAccount/";

        var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("x-access-token", token);

        await www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
            return null;
        }
        Debug.Log(www.downloadHandler.text);
        var N = JSON.Parse(www.downloadHandler.text);
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
        catch (ArgumentOutOfRangeException ex)
        {
            return false;
        }
    }
    public int createAccount(string userId, string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/create/account/" + userId;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        //Debug.Log("227W");
        try
        {
            using (System.IO.Stream s = request.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    //Debug.Log("233w");
                    //Debug.Log(jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    if (!N["success"].AsBool)
                    {
                        if (jsonResponse.Contains("\"field\":\"locale\"")) return 222;
                        else return 400;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            //Debug.Log (ex);
            return 400;
        }
    }
    public bool bankInfo(string userId, string token, string iban, string swift)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/bank/info/" + userId;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.ContentType = @"application/json";
        using (var stream = request.GetRequestStream())
        {
            byte[] jsonAsBytes = encoding.GetBytes("{\"IBAN\":\"" + iban + "\",\"swift\":\"" + swift + "\"}");
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    //Debug.Log (jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    return N["success"].AsBool;
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
                        //Debug.Log (error);
                    }
                }
            }
            return false;
        }
    }
    public bool isPayoutConfirmed(string payout_id, string token)
    {
        //Get notification of transfer with transactionId=transId passed on params
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/payout/" + payout_id;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        try
        {
            using (System.IO.Stream s = request.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    //Debug.Log("is payout confirmed status: "+N["data"]["status"].Value);
                    if (N["success"].AsBool == true)
                    {
                        if (N["data"]["status"].Value == "paid")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            return false;
            Debug.Log(ex);
        }
    }
    public string transfer(string userId, string token, string amount)
    {
        string url = Endpoint.classesURL + "/payments/transfer/" + userId;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.ContentType = @"application/json";
        using (var stream = request.GetRequestStream())
        {
            byte[] jsonAsBytes = encoding.GetBytes("{\"amount\":\"" + amount + "\"}");
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    //Debug.Log (jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    try
                    {
                        if (N["data"]["code"].AsInt == 0)
                        {
                            return N["data"]["transaction_id"].Value;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (FormatException fe)
                    {
                        return null;
                    }
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
                        //Debug.Log (error);
                    }
                }
            }
            return null;
        }
    }
    public void transferForOperator(string userId, string amount)
    {
        string url = Endpoint.classesURL + "/payments/transfer/operator/" + userId;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = @"application/json";
        using (var stream = request.GetRequestStream())
        {
            byte[] jsonAsBytes = encoding.GetBytes("{\"amount\":\"" + amount + "\"}");
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    //Debug.Log (jsonResponse);
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
                        //Debug.Log (error);
                    }
                }
            }
        }
    }
    public async Task<string> Payout(string token, float amount)
    {
        string url = Endpoint.classesURL + "/payments/payout/";
        WWWForm form = new WWWForm();
        form.AddField("amount", amount.ToString());
        var www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("x-access-token", token);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";

        await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
            return "error";
        }
        Debug.Log(www.downloadHandler.text);

        var N = JSON.Parse(www.downloadHandler.text);
        if (N["success"].AsBool == true)
        {
            return WITHDRAW_SUCCEEDED_STATUS;
        }
        else return (!string.IsNullOrEmpty(N["error"]["code"])) 
                ? N["error"]["code"].Value 
                : 
                "error" ;



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

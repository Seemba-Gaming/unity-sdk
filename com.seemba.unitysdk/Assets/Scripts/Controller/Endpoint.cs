using System;
using UnityEngine;
namespace SeembaSDK
{
    public class Endpoint : MonoBehaviour
    {
        public static string stripeURL = "https://api.stripe.com/v1";
        public static string locationURL = "https://ipinfo.io/json/";
        public static string laguagesURL = "https://assets.seemba.com/lang";

        public static string classesURL = "https://api.seemba.com/api/v1";
        public static string flagsURL = "https://api.seemba.com/flags/";
        public static string TokenizationAccount = "pk_live_0B1ByioGzmNDXyMt7QEgZJGA00S9D2jGro";
        public static string SdkVersion = "1.1.1 ";

        public static void Init()
        {
            if(Seemba.Get.DevelopmentMode)
            {
                classesURL = "https://api-staging.seemba.com/api/v1";
                flagsURL = "https://api-staging.seemba.com/flags/";
                TokenizationAccount = "pk_test_A8fKBAogt5UIexspxnivPLGw00HslhmxSr";
            }
        }
    }
}

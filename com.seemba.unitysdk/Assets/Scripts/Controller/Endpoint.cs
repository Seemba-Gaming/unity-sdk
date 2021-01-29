using System;
using UnityEngine;
namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class Endpoint : MonoBehaviour
    {
        public static string stripeURL = "https://api.stripe.com/v1";
        public static string locationURL = "https://ipinfo.io/json/";
        public static string laguagesURL = "https://seemba-1556155050.s3.amazonaws.com/sdk/laguages_files";

        public static string classesURL = "https://api.seemba.com/api/v1";
        public static string flagsURL = "https://api.seemba.com/flags/";
        public static string TokenizationAccount = "pk_live_0B1ByioGzmNDXyMt7QEgZJGA00S9D2jGro";
        public static string SdkVersion = "1.0.26";

        public static void Init()
        {
            if(Seemba.Get.DevelopmentMode)
            {
                stripeURL = "https://api.stripe.com/v1";
                locationURL = "https://ipinfo.io/json/";
                laguagesURL = "https://seemba-1556155050.s3.amazonaws.com/sdk/laguages_files";

                classesURL = "https://api.seemba.com/api/v1";
                flagsURL = "https://api.seemba.com/flags/";
                TokenizationAccount = "pk_live_0B1ByioGzmNDXyMt7QEgZJGA00S9D2jGro";
            }
        }
    }
}
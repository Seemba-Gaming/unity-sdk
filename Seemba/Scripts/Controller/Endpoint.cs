using System;
using UnityEngine;
namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class Endpoint : MonoBehaviour
    {
        public static string stripeURL;
        public static string locationURL;
        public static string laguagesURL;

        public static string classesURL;
        public static string flagsURL;
        public static string TokenizationAccount;

        public static void Init()
        {
            if(Seemba.Get.DevMode)
            {
                stripeURL = "https://api.stripe.com/v1";
                locationURL = "https://ipinfo.io/json/";
                laguagesURL = "https://seemba-1556155050.s3.amazonaws.com/sdk/laguages_files";

                classesURL = "https://api.seemba.com/api/v1";
                flagsURL = "https://api.seemba.com/flags/";
                TokenizationAccount = "pk_live_0B1ByioGzmNDXyMt7QEgZJGA00S9D2jGro";
            }
            else
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

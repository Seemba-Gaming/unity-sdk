using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CLSCompliant(false)]
public class Endpoint : MonoBehaviour 
{
	public static string cloudURL="http://seembaapi.herokuapp.com/api/functions/";
    public static string stripeURL="https://api.stripe.com/v1"; 
	public static string locationURL="http://ipinfo.io/json/";
	public static string laguagesURL= "https://seemba-1556155050.s3.amazonaws.com/sdk/laguages_files";
    public static string phonePrefixURL = "http://www.geognos.com/api/en/countries/info";

    #region Live
    public static string classesURL = "https://api.seemba.com/api/v1";
    public static string TokenizationAccount="pk_live_0B1ByioGzmNDXyMt7QEgZJGA00S9D2jGro";
    #endregion

    //#region Test
    //public static string classesURL = "http://api-staging.seemba.com/api/v1";
    //public static string TokenizationAccount= "pk_test_A8fKBAogt5UIexspxnivPLGw00HslhmxSr";
    //#endregion
}

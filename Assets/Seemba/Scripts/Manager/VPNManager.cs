using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using SimpleJSON;
using System.IO;
public class VPNManager  {
	public bool isVpnConnected(){
		string negativeResponse = "no";
		string url = Endpoint.classesURL+"/users/check/vpn";
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
		request.Method = "GET";
		try {
			HttpWebResponse response;
			using (response = (HttpWebResponse)request.GetResponse()) {
				System.IO.Stream s = response.GetResponseStream();
				using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
				{
					var jsonResponse = sr.ReadToEnd();
					var N=JSON.Parse(jsonResponse);
					if(N["body"]["proxy"].Value.ToString()==negativeResponse)return false;
					else return true;
				}
			}
		}
		catch (WebException ex) {
			if (ex.Response != null) {
				using (var errorResponse = (HttpWebResponse)ex.Response) {
					using (var reader = new StreamReader(errorResponse.GetResponseStream())) {
						string error = reader.ReadToEnd();
						//Debug.Log(error);
					}
				}
			}
			return false;
		}
	}
}

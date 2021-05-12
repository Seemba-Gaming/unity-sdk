using Newtonsoft.Json;
using UnityEngine;
namespace SeembaSDK
{
	#pragma warning disable 649
	class VpnInfo
    {
		public VpnBody body;
    }
	class VpnBody
    {
		public string ip;
		public string proxy;
    }
	public class VPNManager
	{
		public async System.Threading.Tasks.Task<bool> isVpnConnectedAsync()
		{
			string negativeResponse = "no";
			string url = Endpoint.classesURL + "/users/check/vpn";
			var responseText = await SeembaWebRequest.Get.HttpsGet(url);
			VpnInfo response = JsonConvert.DeserializeObject<VpnInfo>(responseText);

            if (response.body.proxy == negativeResponse)
            {
                return false;
            }
            else
            {
                return true;
            }
		}
	}
}

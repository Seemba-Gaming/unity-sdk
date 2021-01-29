using SimpleJSON;
namespace SeembaSDK
{
	public class VPNManager
	{
		public async System.Threading.Tasks.Task<bool> isVpnConnectedAsync()
		{
			string negativeResponse = "no";
			string url = Endpoint.classesURL + "/users/check/vpn";
			var response = await SeembaWebRequest.Get.HttpsGet(url);
			var N = JSON.Parse(response);
			if (N["body"]["proxy"].Value.ToString() == negativeResponse)
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

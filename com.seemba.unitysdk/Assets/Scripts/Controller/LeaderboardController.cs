using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SeembaSDK
{
    #pragma warning disable CS4014
    [CLSCompliant(false)]
    public class LeaderBoardItem
    {
        public User user;
        public int score;
    }
    [CLSCompliant(false)]
    public class LeaderboardController : MonoBehaviour
    {
        #region Script Parameters
        public GameObject LeaderboarditemPrefab;
        public Transform Content;
        #endregion

        #region Fields
        private LeaderBoardItem[] mLeaderBoardItems;
        #endregion

        #region Methods
        public async Task GetLeaderBoardAsync(string period)
        {
            LoaderManager.Get.LoaderController.ShowLoader();
            //await some shit
            mLeaderBoardItems = await GetLeaberboard(period);
            FillLeaderboardAsync(mLeaderBoardItems);
            LoaderManager.Get.LoaderController.HideLoader();
        }
        public async Task<LeaderBoardItem[]> GetLeaberboard(string period)
        {
            string url = Endpoint.classesURL + "/analytics/leaderboard-"+ period + "/" + GamesManager.GAME_ID;
            WWWForm form = new WWWForm();
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            SeembaResponse<LeaderBoardItem[]> responseData = JsonConvert.DeserializeObject<SeembaResponse<LeaderBoardItem[]>>(response);
            return responseData.data;
        }
        #endregion

        #region Implementation
        private void FillLeaderboardAsync(LeaderBoardItem[] LeaderBoardItems)
        {
            ResetContent();
            foreach(LeaderBoardItem item in LeaderBoardItems)
            {
                var leaderboardItem = Instantiate(LeaderboarditemPrefab, Content);
                leaderboardItem.GetComponent<LeaderboardItemController>().InitAsync(item);
            }
        }

        private void ResetContent()
        {
            foreach (Transform item in Content)
            {
                Destroy(item.gameObject);
            }
        }
        #endregion


    }
}

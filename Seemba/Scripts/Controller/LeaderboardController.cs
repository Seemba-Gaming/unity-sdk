using System;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class LeaderBoardItem
    {
        public User user;
        public int score;
    }
    [CLSCompliant(false)]
    public class LeaderboardController : MonoBehaviour
    {
        private LeaderBoardItem[] LeaderBoardItems;
        private GameObject LeaderboarditemPrefab;
        private Transform Content;

        public void GetLeaderBoard(string period)
        {
            LoaderManager.Get.LoaderController.ShowLoader();
            //await some shit
            FillLeaderboard(LeaderBoardItems);
            LoaderManager.Get.LoaderController.HideLoader();
        }

        private void FillLeaderboard(LeaderBoardItem[] LeaderBoardItems)
        {
            foreach(LeaderBoardItem item in LeaderBoardItems)
            {
                 Instantiate(LeaderboarditemPrefab, Content);
            }
        }
    }
}

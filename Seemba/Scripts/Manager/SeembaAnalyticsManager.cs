using System;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class SeembaAnalyticsManager : MonoBehaviour
    {
        #region Static
        public static SeembaAnalyticsManager Get { get { return sInstance; } }

        private static SeembaAnalyticsManager sInstance;
        #endregion


        #region Unity Methods
        private void Awake()
        {
            sInstance = this;
        }
        #endregion

        #region Methods

        public void GameOpened(string action)
        {
            var props = new Value();
            props["Game Name"] = Application.productName;
            props["Platform"] = Application.platform.ToString();
            SeembaMixpanel.Track(action, props);
        }

        public void SendGameEvent(string action)
        {
            var props = new Value();
            InitProps(props);
            props["Action"] = action;
            SeembaMixpanel.Track(action, props);
        }

        public void SendUserEvent(string action)
        {
            var props = new Value();
            InitProps(props);
            props["User Id"] = UserManager.Get.CurrentUser._id;
            props["Action"] = action;
            SeembaMixpanel.Track(action, props);
        }

        public void SendDuelEvent(string action, string duelId, float score)
        {
            var props = new Value(); 
            InitProps(props);
            props["User Id"] = UserManager.Get.CurrentUser._id;
            props["challenge Id"] = duelId;
            props["score"] = score;
            props["Action"] = action;
            SeembaMixpanel.Track(action, props);
        }

        public void SendTournamentEvent(string action, string tournamentId, float score)
        {
            var props = new Value();
            InitProps(props);
            props["User Id"] = UserManager.Get.CurrentUser._id;
            props["Tournament Id"] = tournamentId;
            props["score"] = score;
            props["Action"] = action;
            SeembaMixpanel.Track(action, props);
        }

        public void SendCreditEvent(string action, string amount)
        {
            var props = new Value();
            InitProps(props);
            props["User Id"] = UserManager.Get.CurrentUser._id;
            props["Credited Amount"] = amount;
            props["Action"] = action;
            SeembaMixpanel.Track(action, props);
        }

        public void InitProps(Value value)
        {
            value["Game Name"] = GamesManager.GAME_NAME;
            value["Game Id"] = GamesManager.GAME_ID;
            value["Platform"] = Application.platform.ToString();
        }
        #endregion
    }
}

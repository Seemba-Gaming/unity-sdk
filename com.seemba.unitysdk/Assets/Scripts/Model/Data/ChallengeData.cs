using System;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class ChallengeData
    {
        public bool success;
        public string message;
        public Challenge data;

        public ChallengeData(bool success, string message, Challenge data)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }
    }
    [Serializable]
    class ChallengeListData
    {
        public bool success;
        public string message;
        public Challenge[] data;

        public ChallengeListData(bool success, string message, Challenge[] data)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }
    }
}


using System;
namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class Challenge
    {
        //public string objectId;
        public string _id;
        public string status, challenge_type;
        public string gain;
        public User matched_user_1, matched_user_2;
        public float? user_1_score, user_2_score;
        public Game game;
        public string gain_type;
        public int level;
        public string winner_user;
        public string CreatedAt;
        public int? game_level;

        public Challenge(string _id, User matched_user_1, User matched_user_2, float? user_1_score, float? user_2_score, string challenge_type, Game game, string status, string gain, string gain_type, int level, string CreatedAt, string winner_user, int? game_level)
        {
            this._id = _id;
            this.gain = gain;
            this.matched_user_1 = matched_user_1;
            this.matched_user_2 = matched_user_2;
            this.user_1_score = user_1_score;
            this.user_2_score = user_2_score;
            this.game = game;
            this.challenge_type = challenge_type;
            this.status = status;
            this.gain_type = gain_type;
            this.level = level;
            this.CreatedAt = CreatedAt;
            this.winner_user = winner_user;
            this.game_level = game_level;

        }
        public Challenge()
        {
        }

    }
}
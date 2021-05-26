using UnityEngine;

namespace SeembaSDK
{
    public class ResultController : MonoBehaviour
    {
        #region static
        [Header("Result Lose")]
        public static string YOU_LOST_TEXT;// =  TranslationManager._instance.Get("you_lost");
        public static string DEFEAT_TEXT;// = TranslationManager._instance.Get("defeat");
        public static string YOUR_OPPONENT_WON_TEXT;// =  TranslationManager._instance.Get("your_opponent_won");
        public static string DO_NOT_GIVE_UP_TEXT;// = TranslationManager._instance.Get("dont_give_up");
        [Header("Result Win")]
        public static string YOU_WIN_TEXT;// =  TranslationManager._instance.Get("you_win");
        public static string VICTORY_TEXT;//=  TranslationManager._instance.Get("victory");
        public static string CONGRATS_TEXT;// = TranslationManager._instance.Get("congratulation");
        [Header("Result Draw")]
        public static string TRY_AGAIN;// = TranslationManager._instance.Get("try_again");
        public static string DRAW_TEXT;// = TranslationManager._instance.Get("equality");
        public static string YOU_WILL_WIN;// = TranslationManager._instance.Get("you_will_win");
        public static string NEVER_GIVE_UP_TEXT;//=  TranslationManager._instance.Get("never_give_up");
        [Header("Result Waiting")]
        public static string WAITING_FOR;// =  TranslationManager._instance.Get("waiting_for");
        public static string PLAYER_2;// =  TranslationManager._instance.Get("player_2");
        public static string CONTINUE_NOW;// =  TranslationManager._instance.Get("continue_now");
        public static string AND_GET_RESULT_LATER;//=  TranslationManager._instance.Get("and_get_results_later");
        [Header("Common")]
        public static string PLAY_AGAIN_TEXT;// =  TranslationManager._instance.Get("play_again");
        public static string CONTINUE_TEXT;// =  TranslationManager._instance.Get("continue");
        public static string DATE_TEXT;// =  TranslationManager._instance.Get("date");
        public static string ID_TEXT;// = TranslationManager._instance.Get("ID");
        public static string SCORE_TEXT;// = TranslationManager._instance.Get("score");
        public static string BUBBLES;//= TranslationManager._instance.Get("bubbles");
        public static bool InitDone = false;
        #endregion

        public static void InitTexts()
        {
            TranslationManager._instance.scene = "ResultLose";
            PLAY_AGAIN_TEXT = TranslationManager._instance.Get("play_again");
            CONTINUE_TEXT = TranslationManager._instance.Get("continue");
            DATE_TEXT = TranslationManager._instance.Get("date");
            ID_TEXT = TranslationManager._instance.Get("ID");
            SCORE_TEXT = TranslationManager._instance.Get("score");
            YOU_LOST_TEXT = TranslationManager._instance.Get("you_lose");
            DEFEAT_TEXT = TranslationManager._instance.Get("defeat");
            YOUR_OPPONENT_WON_TEXT = TranslationManager._instance.Get("your_opponent_won");
            DO_NOT_GIVE_UP_TEXT = TranslationManager._instance.Get("dont_give_up");

            TranslationManager._instance.scene = "ResultWin";
            YOU_WIN_TEXT = TranslationManager._instance.Get("you_win");
            VICTORY_TEXT = TranslationManager._instance.Get("victory");
            CONGRATS_TEXT = TranslationManager._instance.Get("congratulation");

            TranslationManager._instance.scene = "ResultEquality";
            TRY_AGAIN = TranslationManager._instance.Get("try_again");
            DRAW_TEXT = TranslationManager._instance.Get("equality");
            YOU_WILL_WIN = TranslationManager._instance.Get("you_will_win");
            NEVER_GIVE_UP_TEXT = TranslationManager._instance.Get("never_give_up");

            TranslationManager._instance.scene = "ResultWaiting";
            WAITING_FOR = TranslationManager._instance.Get("waiting_for");
            PLAYER_2 = TranslationManager._instance.Get("player_2");
            CONTINUE_NOW = TranslationManager._instance.Get("continue_now");
            AND_GET_RESULT_LATER = TranslationManager._instance.Get("and_get_results_later");

            TranslationManager._instance.scene = "Home";
            BUBBLES = TranslationManager._instance.Get("bubbles");

            InitDone = true;
        }
    }
}

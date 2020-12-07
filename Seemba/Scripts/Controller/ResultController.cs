using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class ResultController : MonoBehaviour
    {
        #region static
        [Header("Result Lose")]
        public static string YOU_LOST_TEXT;// =  TranslationManager.Get("you_lost");
        public static string DEFEAT_TEXT;// = TranslationManager.Get("defeat");
        public static string YOUR_OPPONENT_WON_TEXT;// =  TranslationManager.Get("your_opponent_won");
        public static string DO_NOT_GIVE_UP_TEXT;// = TranslationManager.Get("dont_give_up");
        [Header("Result Win")]
        public static string YOU_WIN_TEXT;// =  TranslationManager.Get("you_win");
        public static string VICTORY_TEXT;//=  TranslationManager.Get("victory");
        public static string CONGRATS_TEXT;// = TranslationManager.Get("congratulation");
        [Header("Result Draw")]
        public static string TRY_AGAIN;// = TranslationManager.Get("try_again");
        public static string DRAW_TEXT;// = TranslationManager.Get("equality");
        public static string YOU_WILL_WIN;// = TranslationManager.Get("you_will_win");
        public static string NEVER_GIVE_UP_TEXT;//=  TranslationManager.Get("never_give_up");
        [Header("Result Waiting")]
        public static string WAITING_FOR;// =  TranslationManager.Get("waiting_for");
        public static string PLAYER_2;// =  TranslationManager.Get("player_2");
        public static string CONTINUE_NOW;// =  TranslationManager.Get("continue_now");
        public static string AND_GET_RESULT_LATER;//=  TranslationManager.Get("and_get_results_later");
        [Header("Common")]
        public static string PLAY_AGAIN_TEXT;// =  TranslationManager.Get("play_again");
        public static string CONTINUE_TEXT;// =  TranslationManager.Get("continue");
        public static string DATE_TEXT;// =  TranslationManager.Get("date");
        public static string ID_TEXT;// = TranslationManager.Get("ID");
        public static string SCORE_TEXT;// = TranslationManager.Get("score");
        public static string BUBBLES;//= TranslationManager.Get("bubbles");
        public static bool InitDone = false;
        #endregion

        public static void InitTexts()
        {
            TranslationManager.scene = "ResultLose";
            PLAY_AGAIN_TEXT = TranslationManager.Get("play_again");
            CONTINUE_TEXT = TranslationManager.Get("continue");
            DATE_TEXT = TranslationManager.Get("date");
            ID_TEXT = TranslationManager.Get("ID");
            SCORE_TEXT = TranslationManager.Get("score");
            YOU_LOST_TEXT = TranslationManager.Get("you_lose");
            DEFEAT_TEXT = TranslationManager.Get("defeat");
            YOUR_OPPONENT_WON_TEXT = TranslationManager.Get("your_opponent_won");
            DO_NOT_GIVE_UP_TEXT = TranslationManager.Get("dont_give_up");

            TranslationManager.scene = "ResultWin";
            YOU_WIN_TEXT = TranslationManager.Get("you_win");
            VICTORY_TEXT = TranslationManager.Get("victory");
            CONGRATS_TEXT = TranslationManager.Get("congratulation");

            TranslationManager.scene = "ResultEquality";
            TRY_AGAIN = TranslationManager.Get("try_again");
            DRAW_TEXT = TranslationManager.Get("equality");
            YOU_WILL_WIN = TranslationManager.Get("you_will_win");
            NEVER_GIVE_UP_TEXT = TranslationManager.Get("never_give_up");

            TranslationManager.scene = "ResultWaiting";
            WAITING_FOR = TranslationManager.Get("waiting_for");
            PLAYER_2 = TranslationManager.Get("player_2");
            CONTINUE_NOW = TranslationManager.Get("continue_now");
            AND_GET_RESULT_LATER = TranslationManager.Get("and_get_results_later");

            TranslationManager.scene = "Home";
            BUBBLES = TranslationManager.Get("bubbles");

            InitDone = true;
        }
    }
}

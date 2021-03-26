using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
#pragma warning disable 0649
    public class HomeTranslationController : MonoBehaviour
    {
        [Header("Home")]
        [SerializeField]
        private Text ongoing;
        [SerializeField]
        private Text last_results;
        [SerializeField]
        private Text more_duels_lr;
        [Space]
        [Header("HaveFun")]
        [SerializeField]
        private Text[] duels_hf;
        [SerializeField]
        private Text[] multiplayers_hf;
        [SerializeField]
        private Text more_duels_hf,
            less_duels_hf,
            more_tournaments_hf,
            less_tournaments_hf,
            get_bubbles,
            free_bubbles,
            extra_bubbles;
        [Space]
        [Header("WinMoney")]
        [SerializeField]
        private Text[] duels_wm;
        [SerializeField]
        private Text[] multiplayers_wm;
        [SerializeField]
        private Text more_duels_wm,
            less_duels_wm,
            more_tournaments_wm,
            less_tournaments_wm;
        [Space]
        [Header("Setting")]
        [SerializeField]
        private Text account;
        [SerializeField]
        private Text credit,
            withdraw,
            history,
            help_center,
            back_to_game_menu,
            My_Gifts,
            log_out;
        [Space]
        [Header("Account")]
        [SerializeField]
        private Text personal_info;
        [SerializeField]
        private Text
            security,
            id_proof;
        [SerializeField]
        private Text AccountBackButton;
        [Space]
        [Header("Security")]
        [SerializeField]
        private Text email;
        [SerializeField]
        private Text SecurityBackButton;
        [SerializeField]
        private Text[]
            passwords;
        [SerializeField]
        private Text[]
            change;
        [Space]
        [Header("Wallet")]
        [SerializeField]
        private Text WalletSettingBackButton;
        [SerializeField]
        private Text WalletHeaderBackButton;
        [SerializeField]
        private Text select1,select11;
        [SerializeField]
        private Text select2, select3, select4;
        [SerializeField]
        private Text select22, select33, select44;
        [SerializeField]
        private Text[] accountTitle, creditTitle, creditWallet, other_amount, secured_payment;
        [SerializeField]
        private TextMeshProUGUI stripe_fees_settings, stripe_fees_wallet;
        [Space]
        [Header("Withdraw")]
        [SerializeField]
        private Text WithdrawBackButton;
        [SerializeField]
        private Text money;
        [SerializeField]
        private Text withdrawTitle, available_balance, enter_the_amount_to_withdraw;
        [Space]
        [Header("HelpCenter")]
        [SerializeField]
        private Text HelpCenterBackButton;
        [SerializeField]
        private Text legal;
        [SerializeField]
        private Text LegalBackButton;
        [SerializeField]
        private Text contact;
        [SerializeField]
        private Text ContactBackButton;
        [Header("Market")]
        [SerializeField]
        private Text BubblesMarket;

        [Header("Achievements")]
        public Text Achievements;
        public Text Won;
        public Text ToBeWon;
        //Extra Translation 
        public static string WIN = "";
        public static string BUBBLES = "";
        public static string CROWNS = "";
        public static string AT = "";
        public static string GAME_FINISHED = "";
        public static string DUEL = "";
        public static string YOU_WON = "";
        public static string YOU_LOST = "";
        public static string SCORE_DRAW = "";
        public static string VICTORY = "";
        public static string DEFEAT = "";
        public static string EQUALITY = "";
        // Start is called before the first frame update
        private void Start()
        {
            TranslationManager.scene = "Home";
            ContactBackButton.text = TranslationManager.Get("back_button");
            LegalBackButton.text = TranslationManager.Get("back_button");
            HelpCenterBackButton.text = TranslationManager.Get("back_button");
            WithdrawBackButton.text = TranslationManager.Get("back_button");
            WalletSettingBackButton.text = TranslationManager.Get("back_button");
            WalletHeaderBackButton.text = TranslationManager.Get("back_button");
            SecurityBackButton.text = TranslationManager.Get("back_button");
            AccountBackButton.text = TranslationManager.Get("back_button");
            WIN = TranslationManager.Get("win") != string.Empty ? TranslationManager.Get("win") : "WIN";
            BUBBLES = TranslationManager.Get("bubbles") != string.Empty ? TranslationManager.Get("bubbles") : "bubbles";
            CROWNS = TranslationManager.Get("crowns") != string.Empty ? TranslationManager.Get("crowns") : "crowns";
            AT = TranslationManager.Get("at") != string.Empty ? TranslationManager.Get("at") : "at";
            GAME_FINISHED = TranslationManager.Get("game_finished") != string.Empty ? TranslationManager.Get("game_finished") : "game finished";
            DUEL = TranslationManager.Get("duel") != string.Empty ? TranslationManager.Get("duel") : "duel";
            YOU_WON = TranslationManager.Get("you_won") != string.Empty ? TranslationManager.Get("you_won") : "you won";
            YOU_LOST = TranslationManager.Get("you_lost") != string.Empty ? TranslationManager.Get("you_lost") : "you lost";
            SCORE_DRAW = TranslationManager.Get("score_draw") != string.Empty ? TranslationManager.Get("score_draw") : "score draw";
            VICTORY = TranslationManager.Get("victory") != string.Empty ? TranslationManager.Get("victory") : "victory";
            DEFEAT = TranslationManager.Get("defeat") != string.Empty ? TranslationManager.Get("defeat") : "defeat";
            EQUALITY = TranslationManager.Get("equality") != string.Empty ? TranslationManager.Get("equality") : "equality";

            //Home
            ongoing.text = TranslationManager.Get("ongoing") != string.Empty ? TranslationManager.Get("ongoing") : ongoing.text;
            last_results.text = TranslationManager.Get("last_results") != string.Empty ? TranslationManager.Get("last_results") : last_results.text;
            more_duels_lr.text = TranslationManager.Get("more_duels") != string.Empty ? TranslationManager.Get("more_duels") : more_duels_lr.text;

            //HaveFun 
            foreach (Text duel in duels_hf)
            {
                duel.text = TranslationManager.Get("duels") != string.Empty ? TranslationManager.Get("duels") : duel.text;
            }

            more_duels_hf.text = TranslationManager.Get("more_duels") != string.Empty ? TranslationManager.Get("more_duels") : more_duels_hf.text;
            less_duels_hf.text = TranslationManager.Get("less_duels") != string.Empty ? TranslationManager.Get("less_duels") : less_duels_hf.text;

            foreach (Text multiplayers in multiplayers_hf)
            {
                multiplayers.text = TranslationManager.Get("multiplayers") != string.Empty ? TranslationManager.Get("multiplayers") : multiplayers.text;
            }

            more_tournaments_hf.text = TranslationManager.Get("more_tournaments") != string.Empty ? TranslationManager.Get("more_tournaments") : more_tournaments_hf.text;
            less_tournaments_hf.text = TranslationManager.Get("less_tournaments") != string.Empty ? TranslationManager.Get("less_tournaments") : less_tournaments_hf.text;
            get_bubbles.text = TranslationManager.Get("get_bubbles") != string.Empty ? TranslationManager.Get("get_bubbles") : get_bubbles.text;
            free_bubbles.text = TranslationManager.Get("free_bubbles") != string.Empty ? TranslationManager.Get("free_bubbles") : free_bubbles.text;
            extra_bubbles.text = TranslationManager.Get("extra_bubbles") != string.Empty ? TranslationManager.Get("extra_bubbles") : extra_bubbles.text;

            foreach (Text duel in duels_wm)
            {
                duel.text = TranslationManager.Get("duels") != string.Empty ? TranslationManager.Get("duels") : duel.text;
            }

            more_duels_wm.text = TranslationManager.Get("more_duels") != string.Empty ? TranslationManager.Get("more_duels") : more_duels_wm.text;
            less_duels_wm.text = TranslationManager.Get("less_duels") != string.Empty ? TranslationManager.Get("less_duels") : less_duels_wm.text;
            foreach (Text multiplayers in multiplayers_wm)
            {
                multiplayers.text = TranslationManager.Get("multiplayers") != string.Empty ? TranslationManager.Get("multiplayers") : multiplayers.text;
            }

            more_tournaments_wm.text = TranslationManager.Get("more_tournaments") != string.Empty ? TranslationManager.Get("more_tournaments") : more_tournaments_wm.text;
            less_tournaments_wm.text = TranslationManager.Get("less_tournaments") != string.Empty ? TranslationManager.Get("less_tournaments") : less_tournaments_wm.text;

            account.text = TranslationManager.Get("account") != string.Empty ? TranslationManager.Get("account") : account.text;
            credit.text = TranslationManager.Get("credit") != string.Empty ? TranslationManager.Get("credit") : credit.text;
            withdraw.text = TranslationManager.Get("withdraw") != string.Empty ? TranslationManager.Get("withdraw") : withdraw.text;
            history.text = TranslationManager.Get("history") != string.Empty ? TranslationManager.Get("history") : history.text;
            help_center.text = TranslationManager.Get("help_center") != string.Empty ? TranslationManager.Get("help_center") : help_center.text;
            back_to_game_menu.text = TranslationManager.Get("back_to_game_menu") != string.Empty ? TranslationManager.Get("back_to_game_menu") : back_to_game_menu.text;
            log_out.text = TranslationManager.Get("log_out") != string.Empty ? TranslationManager.Get("log_out") : log_out.text;
            //Account
            personal_info.text = TranslationManager.Get("personal_info") != string.Empty ? TranslationManager.Get("personal_info") : personal_info.text;
            security.text = TranslationManager.Get("security") != string.Empty ? TranslationManager.Get("security") : security.text;
            //Security
            email.text = TranslationManager.Get("email") != string.Empty ? TranslationManager.Get("email") : email.text;
            //----> "Security" COMMUN TEXT BETWEEN SECURITY POPUPS
            foreach (Text _change in change)
            {
                _change.text = TranslationManager.Get("change") != string.Empty ? TranslationManager.Get("change") : _change.text;
            }

            foreach (Text password in passwords)
            {
                password.text = TranslationManager.Get("password") != string.Empty ? TranslationManager.Get("password") : password.text;
            }

            //Wallet
            foreach (Text title in accountTitle)
            {
                title.text = TranslationManager.Get("account") != string.Empty ? TranslationManager.Get("account") : title.text;
            }
            stripe_fees_settings.text = TranslationManager.Get("stripe_fee_may_apply");
            stripe_fees_wallet.text = TranslationManager.Get("stripe_fee_may_apply");
            foreach (Text title in creditTitle)
            {
                title.text = TranslationManager.Get("creditTitle") != string.Empty ? TranslationManager.Get("creditTitle") : title.text;
            }

            select1.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select1.text;
            select2.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select2.text;
            select3.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select3.text;
            select4.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select4.text;
            
            select11.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select11.text;
            select22.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select22.text;
            select33.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select33.text;
            select44.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select44.text;

            foreach (Text title in creditWallet)
            {
                title.text = TranslationManager.Get("creditWallet") != string.Empty ? TranslationManager.Get("creditWallet") : title.text;
            }

            foreach (Text title in other_amount)
            {
                title.text = TranslationManager.Get("other_amount") != string.Empty ? TranslationManager.Get("other_amount") : title.text;
            }

            foreach (Text title in secured_payment)
            {
                title.text = TranslationManager.Get("secured_payment") != string.Empty ? TranslationManager.Get("secured_payment") : title.text;
            }

            //HelpCenter
            legal.text = TranslationManager.Get("legal") != string.Empty ? TranslationManager.Get("legal") : legal.text;
            contact.text = TranslationManager.Get("contact") != string.Empty ? TranslationManager.Get("contact") : contact.text;
            //Wihdraw
            money.text = TranslationManager.Get("money");
            withdrawTitle.text = TranslationManager.Get("withdraw");
            available_balance.text = TranslationManager.Get("available_balance");
            enter_the_amount_to_withdraw.text = TranslationManager.Get("enter_the_amount_to_withdraw");
            BubblesMarket.text = TranslationManager.Get("bubbles_market");

            //Achievements
            Achievements.text = TranslationManager.Get("achievements");
            Won.text = TranslationManager.Get("won");
            ToBeWon.text = TranslationManager.Get("to_be_won");
        }
    }
}

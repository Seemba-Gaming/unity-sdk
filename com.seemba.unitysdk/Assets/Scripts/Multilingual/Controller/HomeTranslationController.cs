using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    #pragma warning disable 649
    public class HomeTranslationController : MonoBehaviour
    {
        [Header("Home")]
        [SerializeField]
        private TextMeshProUGUI ongoing;
        [SerializeField]
        private TextMeshProUGUI last_results;
        [SerializeField]
        private Text more_duels_lr;
        [SerializeField]
        private TextMeshProUGUI No_OnGoing_Challenges;
        [SerializeField]
        private TextMeshProUGUI No_LastResults;
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
        //[Header("Withdraw")]
        //[SerializeField]
        //private Text WithdrawBackButton;
        //[SerializeField]
        //private Text money;
        //[SerializeField]
        //private Text withdrawTitle, available_balance, enter_the_amount_to_withdraw;
        //[Space]
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
        public TextMeshProUGUI Won;
        public TextMeshProUGUI ToBeWon;
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
            TranslationManager._instance.scene = "Home";
 
            ContactBackButton.text = TranslationManager._instance.Get("back_button");
            LegalBackButton.text = TranslationManager._instance.Get("back_button");
            HelpCenterBackButton.text = TranslationManager._instance.Get("back_button");
            //WithdrawBackButton.text = TranslationManager._instance.Get("back_button");
            WalletSettingBackButton.text = TranslationManager._instance.Get("back_button");
            WalletHeaderBackButton.text = TranslationManager._instance.Get("back_button");
            SecurityBackButton.text = TranslationManager._instance.Get("back_button");
            AccountBackButton.text = TranslationManager._instance.Get("back_button");
            WIN = TranslationManager._instance.Get("win") != string.Empty ? TranslationManager._instance.Get("win") : "WIN";
            BUBBLES = TranslationManager._instance.Get("bubbles") != string.Empty ? TranslationManager._instance.Get("bubbles") : "bubbles";
            CROWNS = TranslationManager._instance.Get("crowns") != string.Empty ? TranslationManager._instance.Get("crowns") : "crowns";
            AT = TranslationManager._instance.Get("at") != string.Empty ? TranslationManager._instance.Get("at") : "at";
            GAME_FINISHED = TranslationManager._instance.Get("game_finished") != string.Empty ? TranslationManager._instance.Get("game_finished") : "game finished";
            DUEL = TranslationManager._instance.Get("duel") != string.Empty ? TranslationManager._instance.Get("duel") : "duel";
            YOU_WON = TranslationManager._instance.Get("you_won") != string.Empty ? TranslationManager._instance.Get("you_won") : "you won";
            YOU_LOST = TranslationManager._instance.Get("you_lost") != string.Empty ? TranslationManager._instance.Get("you_lost") : "you lost";
            SCORE_DRAW = TranslationManager._instance.Get("score_draw") != string.Empty ? TranslationManager._instance.Get("score_draw") : "score draw";
            VICTORY = TranslationManager._instance.Get("victory") != string.Empty ? TranslationManager._instance.Get("victory") : "victory";
            DEFEAT = TranslationManager._instance.Get("defeat") != string.Empty ? TranslationManager._instance.Get("defeat") : "defeat";
            EQUALITY = TranslationManager._instance.Get("equality") != string.Empty ? TranslationManager._instance.Get("equality") : "equality";

            //Home
            ongoing.text = TranslationManager._instance.Get("ongoing") != string.Empty ? TranslationManager._instance.Get("ongoing") : ongoing.text;
            last_results.text = TranslationManager._instance.Get("last_results") != string.Empty ? TranslationManager._instance.Get("last_results") : last_results.text;
            more_duels_lr.text = TranslationManager._instance.Get("more_duels") != string.Empty ? TranslationManager._instance.Get("more_duels") : more_duels_lr.text;

            //HaveFun 
            foreach (Text duel in duels_hf)
            {
                duel.text = TranslationManager._instance.Get("duels") != string.Empty ? TranslationManager._instance.Get("duels") : duel.text;
            }

            more_duels_hf.text = TranslationManager._instance.Get("more_duels") != string.Empty ? TranslationManager._instance.Get("more_duels") : more_duels_hf.text;
            less_duels_hf.text = TranslationManager._instance.Get("less_duels") != string.Empty ? TranslationManager._instance.Get("less_duels") : less_duels_hf.text;

            foreach (Text multiplayers in multiplayers_hf)
            {
                multiplayers.text = TranslationManager._instance.Get("multiplayers") != string.Empty ? TranslationManager._instance.Get("multiplayers") : multiplayers.text;
            }

            more_tournaments_hf.text = TranslationManager._instance.Get("more_tournaments") != string.Empty ? TranslationManager._instance.Get("more_tournaments") : more_tournaments_hf.text;
            less_tournaments_hf.text = TranslationManager._instance.Get("less_tournaments") != string.Empty ? TranslationManager._instance.Get("less_tournaments") : less_tournaments_hf.text;
            get_bubbles.text = TranslationManager._instance.Get("get_bubbles") != string.Empty ? TranslationManager._instance.Get("get_bubbles") : get_bubbles.text;
            free_bubbles.text = TranslationManager._instance.Get("free_bubbles") != string.Empty ? TranslationManager._instance.Get("free_bubbles") : free_bubbles.text;
            extra_bubbles.text = TranslationManager._instance.Get("extra_bubbles") != string.Empty ? TranslationManager._instance.Get("extra_bubbles") : extra_bubbles.text;

            foreach (Text duel in duels_wm)
            {
                duel.text = TranslationManager._instance.Get("duels") != string.Empty ? TranslationManager._instance.Get("duels") : duel.text;
            }

            more_duels_wm.text = TranslationManager._instance.Get("more_duels") != string.Empty ? TranslationManager._instance.Get("more_duels") : more_duels_wm.text;
            less_duels_wm.text = TranslationManager._instance.Get("less_duels") != string.Empty ? TranslationManager._instance.Get("less_duels") : less_duels_wm.text;
            foreach (Text multiplayers in multiplayers_wm)
            {
                multiplayers.text = TranslationManager._instance.Get("multiplayers") != string.Empty ? TranslationManager._instance.Get("multiplayers") : multiplayers.text;
            }

            more_tournaments_wm.text = TranslationManager._instance.Get("more_tournaments") != string.Empty ? TranslationManager._instance.Get("more_tournaments") : more_tournaments_wm.text;
            less_tournaments_wm.text = TranslationManager._instance.Get("less_tournaments") != string.Empty ? TranslationManager._instance.Get("less_tournaments") : less_tournaments_wm.text;

            account.text = TranslationManager._instance.Get("account") != string.Empty ? TranslationManager._instance.Get("account") : account.text;
            credit.text = TranslationManager._instance.Get("credit") != string.Empty ? TranslationManager._instance.Get("credit") : credit.text;
            withdraw.text = TranslationManager._instance.Get("withdraw") != string.Empty ? TranslationManager._instance.Get("withdraw") : withdraw.text;
            history.text = TranslationManager._instance.Get("history") != string.Empty ? TranslationManager._instance.Get("history") : history.text;
            help_center.text = TranslationManager._instance.Get("help_center") != string.Empty ? TranslationManager._instance.Get("help_center") : help_center.text;
            back_to_game_menu.text = TranslationManager._instance.Get("back_to_game_menu") != string.Empty ? TranslationManager._instance.Get("back_to_game_menu") : back_to_game_menu.text;
            My_Gifts.text = TranslationManager._instance.Get("my_gifts") != string.Empty ? TranslationManager._instance.Get("my_gifts") : My_Gifts.text;
            log_out.text = TranslationManager._instance.Get("log_out") != string.Empty ? TranslationManager._instance.Get("log_out") : log_out.text;
            //Account
            personal_info.text = TranslationManager._instance.Get("personal_info") != string.Empty ? TranslationManager._instance.Get("personal_info") : personal_info.text;
            security.text = TranslationManager._instance.Get("security") != string.Empty ? TranslationManager._instance.Get("security") : security.text;
            //Security
            email.text = TranslationManager._instance.Get("email") != string.Empty ? TranslationManager._instance.Get("email") : email.text;
            //----> "Security" COMMUN TEXT BETWEEN SECURITY POPUPS
            foreach (Text _change in change)
            {
                _change.text = TranslationManager._instance.Get("change") != string.Empty ? TranslationManager._instance.Get("change") : _change.text;
            }

            foreach (Text password in passwords)
            {
                password.text = TranslationManager._instance.Get("password") != string.Empty ? TranslationManager._instance.Get("password") : password.text;
            }

            //Wallet
            foreach (Text title in accountTitle)
            {
                title.text = TranslationManager._instance.Get("account") != string.Empty ? TranslationManager._instance.Get("account") : title.text;
            }
            stripe_fees_settings.text = TranslationManager._instance.Get("stripe_fee_may_apply");
            stripe_fees_wallet.text = TranslationManager._instance.Get("stripe_fee_may_apply");
            foreach (Text title in creditTitle)
            {
                title.text = TranslationManager._instance.Get("creditTitle") != string.Empty ? TranslationManager._instance.Get("creditTitle") : title.text;
            }

            select1.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select1.text;
            select2.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select2.text;
            select3.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select3.text;
            select4.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select4.text;
            
            select11.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select11.text;
            select22.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select22.text;
            select33.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select33.text;
            select44.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : select44.text;

            foreach (Text title in creditWallet)
            {
                title.text = TranslationManager._instance.Get("creditTitle") != string.Empty ? TranslationManager._instance.Get("creditTitle") : title.text;
            }

            foreach (Text title in other_amount)
            {
                title.text = TranslationManager._instance.Get("other_amount") != string.Empty ? TranslationManager._instance.Get("other_amount") : title.text;
            }

            foreach (Text title in secured_payment)
            {
                title.text = TranslationManager._instance.Get("secured_payment") != string.Empty ? TranslationManager._instance.Get("secured_payment") : title.text;
            }

            //HelpCenter
            legal.text = TranslationManager._instance.Get("legal") != string.Empty ? TranslationManager._instance.Get("legal") : legal.text;
            contact.text = TranslationManager._instance.Get("contact") != string.Empty ? TranslationManager._instance.Get("contact") : contact.text;
            //Wihdraw
            //money.text = TranslationManager._instance.Get("money");
            //withdrawTitle.text = TranslationManager._instance.Get("withdraw");
            //available_balance.text = TranslationManager._instance.Get("available_balance");
            //enter_the_amount_to_withdraw.text = TranslationManager._instance.Get("enter_the_amount_to_withdraw");
            BubblesMarket.text = TranslationManager._instance.Get("bubbles_market");

            //Achievements
            Achievements.text = TranslationManager._instance.Get("achievements");
            Won.text = TranslationManager._instance.Get("won");
            ToBeWon.text = TranslationManager._instance.Get("to_be_won");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
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
        log_out;
    [Space]
    [Header("Account")]
    [SerializeField]
    private Text personal_info;
    [SerializeField]
    private Text
        security,
        id_proof;
    [Space]
    [Header("Security")]
    [SerializeField]
    private Text email;
    [SerializeField]
    private Text[]
        passwords;
    [SerializeField]
    private Text[]
        change;
    [Space]
    [Header("Wallet")]
    [SerializeField]
    private Text select1;
    [SerializeField]
    private Text select2, select3, select4, selected1, selected2, selected3, selected4;
    [SerializeField]
    private Text[] accountTitle, creditTitle, creditWallet, other_amount, secured_payment;
    [Space]
    [Header("Withdraw")]
    [SerializeField]
    private Text money;
    [SerializeField]
    private Text withdrawTitle, available_balance, enter_the_amount_to_withdraw;
    [Space]
    [Header("HelpCenter")]
    [SerializeField]
    private Text legal;
    [SerializeField]
    private Text contact;
    //Extra Translation 
    public static string WIN = "";
    public static string BUBBLES = "";
    public static string AT = "";
    public static string GAME_FINISHED = "";
    public static string DUEL = "";
    public static string YOU_WON = "";
    public static string YOU_LOST = "";
    public static string SCORE_DRAW = "";
    public static string VICTORY = "";
    public static string DEFEAT = "";
    public static string EQUALITY = "";
    [Header("Challenges")]
    [Header("*********HaveFun*********")]
    [Header("--------------------POPUPS--------------------")]
    [SerializeField]
    private Text bubble_challenge;
    [SerializeField]
    private Text
            bubble_selection,
            bubble_win_and_get,
            bubble_entry_fee,
            bubble_go;
    [Header("Insufficient Bubbles")]
    [SerializeField]
    private Text insufficient;
    [SerializeField]
    private Text
            bubbles,
            get_free_bubbles,
            got_it;
    [Header("UnknownDevice")]
    [SerializeField]
    private Text unknown;
    [SerializeField]
    private Text
            device,
            sorry_ads_not_supported,
            got_it_unknown;
    [Header("Payment Failed")]
    [Header("*********WinMoney*********")]
    [SerializeField]
    private Text payment_failed;
    [SerializeField]
    private Text
            please_make_sur,
            _continue_payment_failed;
    [Header("Withdrawal Confirmed")]
    [SerializeField]
    private Text withdrawal;
    [SerializeField]
    private Text
            confirmed,
            you_will_receive,
            working_days,
            great;
    [Header("Congrat")]
    [SerializeField]
    private Text congratulations;
    [SerializeField]
    private Text
            transaction_accepted,
            ok;
    [Header("VPN")]
    [SerializeField]
    private Text desactivate_vpn;
    [SerializeField]
    private Text
            your_vpn,
            you_cant_play_vpn,
            got_it_vpn;
    [Header("Dev Setting")]
    [SerializeField]
    private Text desactivate_dev_mode;
    [SerializeField]
    private Text
            The_development_settings,
            you_cant_play_dev_mode,
            got_it_dev_mode;
    [Header("Prohibited Location")]
    [SerializeField]
    private Text prohibited;
    [SerializeField]
    private Text
            location,
            real_money_tournaments,
            got_it_prohibited;
    [Header("Challenge")]
    [SerializeField]
    private Text cash_challenge;
    [SerializeField]
    private Text
            cash_selection,
            cash_win_and_get,
            cash_entry_fee,
            cash_go;
    [Header("Insufficient balance")]
    [SerializeField]
    private Text cash_insufficient;
    [SerializeField]
    private Text
            balance,
            credit_your_account,
            cash_credit;
    [Header("Age Verification")]
    [Header("*********Wallet*********")]
    [SerializeField]
    private Text age;
    [SerializeField]
    private Text
            verification,
            enter_age,
            by_clicking,
            _continue_age_verification;
    [Header("Age Too Young")]
    [SerializeField]
    private Text sorry;
    [SerializeField]
    private Text
            you_are_too_young,
            we_apologize,
            got_it_to_young;
    [Header("Payment")]
    [SerializeField]
    private Text select;
    [SerializeField]
    private Text
            payment_method,
            _continue_payment_method;
    [Header("Prohibited Location")]
    [SerializeField]
    private Text _prohibited;
    [SerializeField]
    private Text
            _location,
            _credit_isnot_authorized,
            got_it_prohibited_location;
    [Header("Missing Info")]
    [Header("*********Withdraw*********")]
    [SerializeField]
    private Text unverified;
    [SerializeField]
    private Text
            _account,
            please_provide,
            got_it_missing_info;
    [Header("Prohibited Location")]
    [SerializeField]
    private Text _prohibited_withdraw;
    [SerializeField]
    private Text
            _location_withdraw,
            __withdraw_isnot_authorized,
            got_it_prohibited_location_withdraw;
    [Header("Current Password")]
    [Header("*********Security*********")]
    [SerializeField]
    private Text
            forgotten;
    [SerializeField]
    private Text
            next,
            current_password;
    [Header("Update Password")]
    [SerializeField]
    private Text
            new_password;
    [SerializeField]
    private Text
            confirm_password,
            confirm_update_password;
    [Header("Password Updated")]
    [SerializeField]
    private Text
           changed;
    [SerializeField]
    private Text
           your_password,
           _great;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "Home";
        //Extra Translation 
        WIN = TranslationManager.Get("win") != string.Empty ? TranslationManager.Get("win") : "WIN";
        BUBBLES = TranslationManager.Get("bubbles") != string.Empty ? TranslationManager.Get("bubbles") : "bubbles";
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
        { duel.text = TranslationManager.Get("duels") != string.Empty ? TranslationManager.Get("duels") : duel.text; }
        more_duels_hf.text = TranslationManager.Get("more_duels") != string.Empty ? TranslationManager.Get("more_duels") : more_duels_hf.text;
        less_duels_hf.text = TranslationManager.Get("less_duels") != string.Empty ? TranslationManager.Get("less_duels") : less_duels_hf.text;
        foreach (Text multiplayers in multiplayers_hf)
        { multiplayers.text = TranslationManager.Get("multiplayers") != string.Empty ? TranslationManager.Get("multiplayers") : multiplayers.text; }
        more_tournaments_hf.text = TranslationManager.Get("more_tournaments") != string.Empty ? TranslationManager.Get("more_tournaments") : more_tournaments_hf.text;
        less_tournaments_hf.text = TranslationManager.Get("less_tournaments") != string.Empty ? TranslationManager.Get("less_tournaments") : less_tournaments_hf.text;
        get_bubbles.text = TranslationManager.Get("get_bubbles") != string.Empty ? TranslationManager.Get("get_bubbles") : get_bubbles.text;
        free_bubbles.text = TranslationManager.Get("free_bubbles") != string.Empty ? TranslationManager.Get("free_bubbles") : free_bubbles.text;
        extra_bubbles.text = TranslationManager.Get("extra_bubbles") != string.Empty ? TranslationManager.Get("extra_bubbles") : extra_bubbles.text;
        //----> "HaveFun" POPUPS : Challenges
        bubble_challenge.text = TranslationManager.Get("challenge") != string.Empty ? TranslationManager.Get("challenge") : bubble_challenge.text;
        bubble_selection.text = TranslationManager.Get("selection") != string.Empty ? TranslationManager.Get("selection") : bubble_selection.text;
        bubble_win_and_get.text = TranslationManager.Get("win_and_get") != string.Empty ? TranslationManager.Get("win_and_get") : bubble_win_and_get.text;
        bubble_entry_fee.text = TranslationManager.Get("entry_fee") != string.Empty ? TranslationManager.Get("entry_fee") : bubble_entry_fee.text;
        bubble_go.text = TranslationManager.Get("go") != string.Empty ? TranslationManager.Get("go") : bubble_go.text;
        //----> "HaveFun" POPUPS : Insufficient Bubbles
        insufficient.text = TranslationManager.Get("insufficient") != string.Empty ? TranslationManager.Get("insufficient") : insufficient.text;
        bubbles.text = BUBBLES;
        get_free_bubbles.text = TranslationManager.Get("get_free_bubbles") != string.Empty ? TranslationManager.Get("get_free_bubbles") : get_free_bubbles.text;
        got_it.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it.text;
        //----> "HaveFun" POPUPS : Unknown Device
        unknown.text = TranslationManager.Get("unknown") != string.Empty ? TranslationManager.Get("unknown") : unknown.text;
        device.text = TranslationManager.Get("device") != string.Empty ? TranslationManager.Get("device") : device.text;
        sorry_ads_not_supported.text = TranslationManager.Get("sorry_ads_not_supported") != string.Empty ? TranslationManager.Get("sorry_ads_not_supported") : sorry_ads_not_supported.text;
        got_it_unknown.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_unknown.text;
        //WinMoney
        foreach (Text duel in duels_wm)
        { duel.text = TranslationManager.Get("duels") != string.Empty ? TranslationManager.Get("duels") : duel.text; }
        more_duels_wm.text = TranslationManager.Get("more_duels") != string.Empty ? TranslationManager.Get("more_duels") : more_duels_wm.text;
        less_duels_wm.text = TranslationManager.Get("less_duels") != string.Empty ? TranslationManager.Get("less_duels") : less_duels_wm.text;
        foreach (Text multiplayers in multiplayers_wm)
        { multiplayers.text = TranslationManager.Get("multiplayers") != string.Empty ? TranslationManager.Get("multiplayers") : multiplayers.text; }
        more_tournaments_wm.text = TranslationManager.Get("more_tournaments") != string.Empty ? TranslationManager.Get("more_tournaments") : more_tournaments_wm.text;
        less_tournaments_wm.text = TranslationManager.Get("less_tournaments") != string.Empty ? TranslationManager.Get("less_tournaments") : less_tournaments_wm.text;
        //----> "WinMoney" POPUPS : Payment failed
        payment_failed.text = TranslationManager.Get("payment_failed") != string.Empty ? TranslationManager.Get("payment_failed") : payment_failed.text;
        please_make_sur.text = TranslationManager.Get("please_make_sur") != string.Empty ? TranslationManager.Get("please_make_sur") : please_make_sur.text;
        _continue_payment_failed.text = TranslationManager.Get("continue") != string.Empty ? TranslationManager.Get("continue") : _continue_payment_failed.text;
        //----> "WinMoney" POPUPS : withdrawal confirmed
        withdrawal.text = TranslationManager.Get("withdrawal") != string.Empty ? TranslationManager.Get("withdrawal") : withdrawal.text;
        confirmed.text = TranslationManager.Get("confirmed") != string.Empty ? TranslationManager.Get("confirmed") : confirmed.text;
        you_will_receive.text = TranslationManager.Get("you_will_receive") != string.Empty ? TranslationManager.Get("you_will_receive") : you_will_receive.text;
        working_days.text = TranslationManager.Get("working_days") != string.Empty ? TranslationManager.Get("working_days") : working_days.text;
        great.text = TranslationManager.Get("great") != string.Empty ? TranslationManager.Get("great") : great.text;
        //----> "WinMoney" POPUPS : congratulations
        congratulations.text = TranslationManager.Get("congratulations") != string.Empty ? TranslationManager.Get("congratulations") : congratulations.text;
        transaction_accepted.text = TranslationManager.Get("transaction_accepted") != string.Empty ? TranslationManager.Get("transaction_accepted") : transaction_accepted.text;
        ok.text = TranslationManager.Get("ok") != string.Empty ? TranslationManager.Get("ok") : ok.text;
        //----> "WinMoney" POPUPS : desactivate vpn
        desactivate_vpn.text = TranslationManager.Get("desactivate") != string.Empty ? TranslationManager.Get("desactivate") : desactivate_vpn.text;
        your_vpn.text = TranslationManager.Get("your_vpn") != string.Empty ? TranslationManager.Get("your_vpn") : your_vpn.text;
        you_cant_play_vpn.text = TranslationManager.Get("you_cant_play_vpn") != string.Empty ? TranslationManager.Get("you_cant_play_vpn") : you_cant_play_vpn.text;
        got_it_vpn.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_vpn.text;
        //----> "WinMoney" POPUPS : desactivate dev mode
        desactivate_dev_mode.text = TranslationManager.Get("desactivate") != string.Empty ? TranslationManager.Get("desactivate") : desactivate_dev_mode.text;
        The_development_settings.text = TranslationManager.Get("The_development_settings") != string.Empty ? TranslationManager.Get("The_development_settings") : The_development_settings.text;
        you_cant_play_dev_mode.text = TranslationManager.Get("you_cant_play_dev_mode") != string.Empty ? TranslationManager.Get("you_cant_play_dev_mode") : you_cant_play_dev_mode.text;
        got_it_dev_mode.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_dev_mode.text;
        //----> "WinMoney" POPUPS : prohibited location
        prohibited.text = TranslationManager.Get("prohibited") != string.Empty ? TranslationManager.Get("prohibited") : prohibited.text;
        location.text = TranslationManager.Get("location") != string.Empty ? TranslationManager.Get("location") : location.text;
        real_money_tournaments.text = TranslationManager.Get("real_money_tournaments") != string.Empty ? TranslationManager.Get("real_money_tournaments") : real_money_tournaments.text;
        got_it_prohibited.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_prohibited.text;
        //----> "WinMoney" POPUPS : cash challenge
        cash_challenge.text = TranslationManager.Get("challenge") != string.Empty ? TranslationManager.Get("challenge") : cash_challenge.text;
        cash_selection.text = TranslationManager.Get("selection") != string.Empty ? TranslationManager.Get("selection") : cash_selection.text;
        cash_win_and_get.text = TranslationManager.Get("win_and_get") != string.Empty ? TranslationManager.Get("win_and_get") : cash_win_and_get.text;
        cash_entry_fee.text = TranslationManager.Get("entry_fee") != string.Empty ? TranslationManager.Get("entry_fee") : cash_entry_fee.text;
        cash_go.text = TranslationManager.Get("cash_go") != string.Empty ? TranslationManager.Get("o") : cash_go.text;
        //----> "WinMoney" POPUPS : insufficient balance
        cash_insufficient.text = TranslationManager.Get("cash_insufficient") != string.Empty ? TranslationManager.Get("cash_insufficient") : cash_insufficient.text;
        balance.text = TranslationManager.Get("balance") != string.Empty ? TranslationManager.Get("balance") : balance.text;
        credit_your_account.text = TranslationManager.Get("credit_your_account") != string.Empty ? TranslationManager.Get("credit_your_account") : credit_your_account.text;
        cash_credit.text = TranslationManager.Get("cash_credit") != string.Empty ? TranslationManager.Get("cash_credit") : cash_credit.text;
        //Setting
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
        id_proof.text = TranslationManager.Get("id_proof") != string.Empty ? TranslationManager.Get("id_proof") : id_proof.text;
        //Security
        email.text = TranslationManager.Get("email") != string.Empty ? TranslationManager.Get("email") : email.text;
        //"change" word moved to "Security POPUPS : Current Password" 
        //----> "Security" COMMUN TEXT BETWEEN SECURITY POPUPS
        foreach (Text _change in change)
        { _change.text = TranslationManager.Get("change") != string.Empty ? TranslationManager.Get("change") : _change.text; }
        foreach (Text password in passwords)
        { password.text = TranslationManager.Get("password") != string.Empty ? TranslationManager.Get("password") : password.text; }
        //----> "Security" POPUPS : Current Password
        forgotten.text = TranslationManager.Get("forgotten") != string.Empty ? TranslationManager.Get("forgotten") : forgotten.text;
        current_password.text = TranslationManager.Get("current_password") != string.Empty ? TranslationManager.Get("current_password") : current_password.text;
        next.text = TranslationManager.Get("next") != string.Empty ? TranslationManager.Get("next") : next.text;
        //----> "Security" POPUPS : Update Password
        new_password.text = TranslationManager.Get("new_password") != string.Empty ? TranslationManager.Get("new_password") : new_password.text;
        confirm_password.text = TranslationManager.Get("confirm_password") != string.Empty ? TranslationManager.Get("confirm_password") : confirm_password.text;
        confirm_update_password.text = TranslationManager.Get("confirm") != string.Empty ? TranslationManager.Get("confirm") : confirm_update_password.text;
        //----> "Security" POPUPS : Password Updated
        changed.text = TranslationManager.Get("changed") != string.Empty ? TranslationManager.Get("changed") : new_password.text;
        your_password.text = TranslationManager.Get("your_password") != string.Empty ? TranslationManager.Get("your_password") : your_password.text;
        _great.text = TranslationManager.Get("great") != string.Empty ? TranslationManager.Get("great") : _great.text;
        //----> "Security" POPUPS : Forget Password
        //----> "Security" POPUPS : Email Sent
        //Wallet
        foreach (Text title in accountTitle)
        { title.text = TranslationManager.Get("account") != string.Empty ? TranslationManager.Get("account") : title.text; }
        foreach (Text title in creditTitle)
        { title.text = TranslationManager.Get("creditTitle") != string.Empty ? TranslationManager.Get("creditTitle") : title.text; }
        select1.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select1.text;
        select2.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select2.text;
        select3.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select3.text;
        select4.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select4.text;
        /*selected1.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : selected1.text;
        selected2.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : selected2.text;
        selected3.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : selected3.text;
        selected4.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : selected4.text;*/
        foreach (Text title in other_amount)
        { title.text = TranslationManager.Get("other_amount") != string.Empty ? TranslationManager.Get("other_amount") : title.text; }
        foreach (Text title in creditWallet)
        { title.text = TranslationManager.Get("creditWallet") != string.Empty ? TranslationManager.Get("creditWallet") : title.text; }
        foreach (Text title in secured_payment)
        { title.text = TranslationManager.Get("secured_payment") != string.Empty ? TranslationManager.Get("secured_payment") : title.text; }
        //----> "Wallet" POPUPS : Age Verification
        age.text = TranslationManager.Get("age") != string.Empty ? TranslationManager.Get("age") : age.text;
        verification.text = TranslationManager.Get("verification") != string.Empty ? TranslationManager.Get("verification") : verification.text;
        enter_age.text = TranslationManager.Get("enter_age") != string.Empty ? TranslationManager.Get("enter_age") : enter_age.text;
        by_clicking.text = TranslationManager.Get("by_clicking") != string.Empty ? TranslationManager.Get("by_clicking") : by_clicking.text;
        _continue_age_verification.text = TranslationManager.Get("continue") != string.Empty ? TranslationManager.Get("continue") : _continue_age_verification.text;
        //----> "Wallet" POPUPS : Age Too Young
        sorry.text = TranslationManager.Get("sorry") != string.Empty ? TranslationManager.Get("sorry") : sorry.text;
        you_are_too_young.text = TranslationManager.Get("you_are_too_young") != string.Empty ? TranslationManager.Get("you_are_too_young") : you_are_too_young.text;
        we_apologize.text = TranslationManager.Get("we_apologize") != string.Empty ? TranslationManager.Get("we_apologize") : we_apologize.text;
        got_it_to_young.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_to_young.text; ;
        //----> "Wallet" POPUPS : Payment
        select.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select.text;
        payment_method.text = TranslationManager.Get("payment_method") != string.Empty ? TranslationManager.Get("payment_method") : payment_method.text;
        _continue_payment_method.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : select.text;
        //----> "Wallet" POPUPS : Prohibited location
        _prohibited.text = TranslationManager.Get("prohibited") != string.Empty ? TranslationManager.Get("prohibited") : _prohibited.text;
        _location.text = TranslationManager.Get("location") != string.Empty ? TranslationManager.Get("location") : _location.text;
        _credit_isnot_authorized.text = TranslationManager.Get("credit_isnot_authorized") != string.Empty ? TranslationManager.Get("credit_isnot_authorized") : _credit_isnot_authorized.text;
        got_it_prohibited_location.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_prohibited_location.text;
        //Withdraw
        money.text = TranslationManager.Get("money") != string.Empty ? TranslationManager.Get("money") : money.text;
        withdrawTitle.text = TranslationManager.Get("withdraw") != string.Empty ? TranslationManager.Get("withdraw") : withdrawTitle.text;
        available_balance.text = TranslationManager.Get("available_balance") != string.Empty ? TranslationManager.Get("available_balance") : available_balance.text;
        enter_the_amount_to_withdraw.text = TranslationManager.Get("enter_the_amount_to_withdraw") != string.Empty ? TranslationManager.Get("enter_the_amount_to_withdraw") : enter_the_amount_to_withdraw.text;
        //----> "Withdraw" POPUPS : Missing info
        unverified.text = TranslationManager.Get("unverified") != string.Empty ? TranslationManager.Get("unverified") : unverified.text;
        _account.text = TranslationManager.Get("account") != string.Empty ? TranslationManager.Get("account") : _account.text;
        please_provide.text = TranslationManager.Get("please_provide") != string.Empty ? TranslationManager.Get("please_provide") : please_provide.text;
        got_it_missing_info.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_missing_info.text;
        //----> "Withdraw" POPUPS : Prohibited location
        _prohibited_withdraw.text = TranslationManager.Get("prohibited") != string.Empty ? TranslationManager.Get("prohibited") : _prohibited_withdraw.text;
        _location_withdraw.text = TranslationManager.Get("location") != string.Empty ? TranslationManager.Get("location") : _location_withdraw.text;
        __withdraw_isnot_authorized.text = TranslationManager.Get("withdrawal_isnot_authorized") != string.Empty ? TranslationManager.Get("withdrawal_isnot_authorized") : __withdraw_isnot_authorized.text;
        got_it_prohibited_location_withdraw.text = TranslationManager.Get("got_it") != string.Empty ? TranslationManager.Get("got_it") : got_it_prohibited_location_withdraw.text;
        //HelpCenter
        legal.text = TranslationManager.Get("legal") != string.Empty ? TranslationManager.Get("legal") : legal.text;
        contact.text = TranslationManager.Get("contact") != string.Empty ? TranslationManager.Get("contact") : contact.text;
    }
}

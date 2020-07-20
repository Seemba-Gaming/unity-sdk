using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PopupsController;

public class PopupsViewPresenter : MonoBehaviour
{
    public class PopupsText
    {
        static PopupsText _Instance;
       


        // Add some elements to the dictionary. There are no
        // duplicate keys, but some of the values are duplicates.

        public PopupsText()
        {
            _Instance = this;

        }
        public static PopupsText getInstance()
        {
            if (_Instance == null) _Instance = new PopupsText();
            return _Instance;
        }



        public object[] insufficient_balance()
        {
            return new object[] { "insufficient", "balance", "credit your account ?", "Credit" };
        }


        public object[] insufficient_bubbles()
        {
            return new object[] { "insufficient", "bubbles", "get free bubbles or watch a commercial to win more bubbles", "GOT IT!" };

        }
        public object[] dev_mode()
        {
            return new object[] { "DESACTIVATE", "The development settings", "You can't play real money tournament when your development settings are enable", "GOT IT!" };
        }
        public object[] prohibited_location()
        {
            return new object[] { "prohibited", "Location", "real money tournaments are not authorized in your territory.", "GOT IT!" };

        }
        public object[] vpn()
        {
            return new object[] { "DESACTIVATE", "YOUR VPN", "You can't play real money tournament with a vpn.", "GOT IT!" };
        }
    }


    private static PopupsViewPresenter _Instance;
    public static object[] _params;
    public static PopupType popup;
    public static bool isActive;
    public static bool isBackgroundActive;



    [Header("blur_background")]
    [SerializeField]
    private Image overlay;

    [Header("popup duels")]
    [SerializeField]
    private Animator popup_duels_animator;
    [SerializeField]
    private Text entry_fee;
    [SerializeField]
    private Text gain;
    [SerializeField]
    private Text bubbles_text;
    [SerializeField]
    private Image bubbles_image;
    [SerializeField]
    private Button cancel_duels;
    [SerializeField]
    private Button confirm_duels;

    [Header("popup error")]
    [SerializeField]
    private Animator popup_error_animator;
    [SerializeField]
    private Text popup_error_title;
    [SerializeField]
    private Text popup_error_subtitle;
    [SerializeField]
    private Text popup_error_main_text;
    [SerializeField]
    private Text popup_error_button_text;
    [SerializeField]
    private Button popup_error_button;



    void Start()
    {

        isActive = true;
        _Instance = this;
        Init();
        Action();


    }
    private void OnDestroy()
    {
        isActive = false;
    }
    void Init()
    {
        switch (popup)
        {
            case PopupType.DUELS:
                ShowPopupDuels();
                break;
            default:
                ShowErrorPopup();
                break;
        }
    }

    public static PopupsViewPresenter getInstance()
    {

        return _Instance;
    }


    public void Action()
    {
        cancel_duels.onClick.AddListener(() =>
        {
            HideSuccessPopupAsync(popup_duels_animator);
        });
        confirm_duels.onClick.AddListener(async () =>
        {
            await HideSuccessPopupAsync(popup_duels_animator);
            if(_params[3].ToString().Equals(ChallengeManager.CHALLENGE_TYPE_1V1))
                StartDuels();
            if (_params[3].ToString().Equals(ChallengeManager.CHALLENGE_TYPE_BRACKET))
                StartTournament();
        });
        popup_error_button.onClick.AddListener(async () =>
        {
            await HideFailurePopupAsync(popup_error_animator);
            switch (CURRENT_POPUP)
            {
                case PopupType.INSUFFICIENT_BALANCE:
                    OpenWallet("WinMoney");
                    break;
            }
        });
    }

    private void OpenWallet(string last_view)
    {
        ViewsEvents view = new ViewsEvents();
        BottomMenuController bottomMenu = BottomMenuController.getInstance();
        bottomMenu.unselectHome();
        bottomMenu.unselectHaveFun();
        bottomMenu.unselectWinMoney();
        bottomMenu.selectSettings();
        BottomMenuController.Hide();
        view.WalletClick(last_view);
    }

    private void StartDuels()
    {
        ChallengeController.getInstance().Play(_params);
    }
    private void StartTournament()
    {
        TournamentController.getInstance().Play(_params);
    }

    public void ShowPopupDuels()
    {
        //Show Background
        ShowOverlay();
        //Setting Params
        entry_fee.text = "Entry fee: " + _params[0];
        gain.text = _params[1].ToString();
        if (_params[2] == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
        {
            bubbles_text.gameObject.SetActive(true);
            bubbles_image.gameObject.SetActive(true);
        }
        else
        {
            entry_fee.text += CurrencyManager.CURRENT_CURRENCY;
            gain.text += CurrencyManager.CURRENT_CURRENCY;
        }
        //get animator
        var animator = GameObject.Find("popup_duels").GetComponent<Animator>();
        //show Popup
        ShowSuccessPopup(animator);
    }
    public void ShowErrorPopup()
    {
        //Show Background
        ShowOverlay();

        //Setting params
        popup_error_title.text = _params[0].ToString();
        popup_error_subtitle.text = _params[1].ToString();
        popup_error_main_text.text = _params[2].ToString();
        popup_error_button_text.text = _params[3].ToString();

        //get animator
        var animator = GameObject.Find("popup_error").GetComponent<Animator>();
        //show Popup
        ShowFailurePopup(animator);
    }
    void ShowOverlay() { overlay.gameObject.SetActive(true); }
    void HideOverlay() { overlay.gameObject.SetActive(false); }
    void ShowFailurePopup(Animator animator) { animator.SetBool("Show Error", true); }
    async Task HideFailurePopupAsync(Animator animator)
    {
        animator.SetBool("Show Error", false);
        while (animator.IsInTransition(0) == false)
        {

            await Task.Delay(200);
        }
        UnloadPopupAsync();
    }
    void ShowSuccessPopup(Animator animator) { animator.SetBool("showSucces", true); }
    async Task HideSuccessPopupAsync(Animator animator)
    {

        animator.SetBool("showSucces", false);
        Task<bool> task = Task.Run(() =>
        {
            return (!animator.IsInTransition(0));
        });

        // Wait until condition is false
        while (!animator.IsInTransition(0))
        {
            await Task.Delay(25);
        }

        UnloadPopupAsync();
    }
    async Task UnloadPopupAsync()
    {
        SceneManager.UnloadScene("Popup");
    }




}

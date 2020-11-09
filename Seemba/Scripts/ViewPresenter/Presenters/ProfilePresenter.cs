using UnityEngine;
using UnityEngine.UI;
using System;

[CLSCompliant(false)]
public class ProfilePresenter : MonoBehaviour
{
    #region Script Parameters
    public string PlayerId;
    public Animator Animator;
    public Image avatar;
    public Image pro;
    public Image drapeau;
    public Text username;
    public Text nbGameWon;
    public Text nbGameWonInARow;
    public Button changeAvatar;
    public GameObject Loading;
    public GameObject verified;
    public GameObject pending;
    public GameObject unverified;
    #endregion

    #region Unity Methods
    async void OnEnable()
    {
        WithdrawManager wm = new WithdrawManager();
        ProfilLastResultListController.profileSceneOpened = true;
        Loading.SetActive(true);
        User user = await UserManager.Get.getUser();

        string token = UserManager.Get.getCurrentSessionToken();
        string userId = UserManager.Get.getCurrentUserId();
        AccountStatus accountStatus = null;

        if (PlayerId == userId)
        {
            accountStatus = await wm.accountVerificationStatus(token);
        }
        string Vectoires = user.victories_count.ToString();
        string SerieVectoires = user.current_victories_count.ToString();

        Loading.SetActive(false);
        if (PlayerId != userId)
        {
            changeAvatar.interactable = false;
        }
        else
        {
            if (accountStatus.verification_status == WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING)
            {
                pending.SetActive(true);
            }
            else if (accountStatus.verification_status == WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED)
            {
                verified.SetActive(true);
            }
            else unverified.SetActive(true);
        }
        username.text = user.username;
        try
        {
            if (user.money_credit > 0)
            {
                pro.gameObject.SetActive(true);
            }
            else
            {
                pro.gameObject.SetActive(false);
            }
        }
        catch (FormatException ex)
        {
            if (user.money_credit > 0)
            {
                pro.gameObject.SetActive(true);
            }
            else
            {
                pro.gameObject.SetActive(false);
            }
        }
        var mTexture = await UserManager.Get.GetFlagBytes(user.country_code);
        drapeau.sprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
        nbGameWon.text = Vectoires;
        nbGameWonInARow.text = SerieVectoires;
    }
    #endregion

    #region Methods
    public async void InitProfile(User user)
    {
        PlayerId = user._id;
        avatar.sprite = await UserManager.Get.getAvatar(user.avatar);
        username.text = user.username;
    }
    #endregion

}

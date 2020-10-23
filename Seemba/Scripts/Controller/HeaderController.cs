using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderController : MonoBehaviour
{
    #region Script Parameters
    public Image Avatar;
    public Text Username;
    public Image ProLabel;
    public Image Flag;
    public Text VirtualMoney;
    public Text RealMoney;
    #endregion

    #region Fields
    User mUser;
    #endregion

    #region Unity Methods
    private async void Start()
    {
        if(UserManager.Get.CurrentUser == null)
        {
            mUser = await UserManager.Get.getUser();
            Init(mUser);
        }
        else
        {
            Init(UserManager.Get.CurrentUser);
        }

    }

    private async void OnEnable()
    {
        User user = await UserManager.Get.getUser();
        UpdateHeaderInfo(user);
    }

    public async void Init(User user)
    {
        Username.text = UserManager.Get.CurrentUser.username;
        if (user.money_credit > 0)
        {
            ProLabel.enabled = true;
        }
        else
        {
            ProLabel.enabled = false;
        }
        VirtualMoney.text = user.bubble_credit.ToString();
        RealMoney.text = user.money_credit.ToString() + CurrencyManager.CURRENT_CURRENCY;
        var countryCode = await UserManager.Get.GetFlagBytes(user.country_code);
        Flag.sprite = Sprite.Create(countryCode, new Rect(0f, 0f, countryCode.width, countryCode.height), Vector2.zero);
        Avatar.sprite = await UserManager.Get.getAvatar(user.avatar);
    }

    public async void UpdateHeaderInfo(User user)
    {
        UserManager.Get.UpdateUserMoneyCredit(user.money_credit.ToString("N2"));
        UserManager.Get.UpdateUserBubblesCredit(user.bubble_credit.ToString());
        Init(user);
    }
    #endregion
}

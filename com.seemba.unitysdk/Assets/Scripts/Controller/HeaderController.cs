using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class HeaderController : MonoBehaviour
    {
        #region Script Parameters
        public Image Avatar;
        public TextMeshProUGUI Username;
        public Image ProLabel;
        public Image Flag;
        public TextMeshProUGUI VirtualMoney;
        public TextMeshProUGUI RealMoney;
        public Animator Market;
        #endregion

        #region Fields
        User mUser;
        #endregion

        #region Unity Methods

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
            VirtualMoney.text = user.bubble_credit.ToString() + " <sprite=0>";
            RealMoney.text = (user.money_credit *100).ToString() + " <sprite=1>";
            var mTexture = await UserManager.Get.GetFlagBytes(user.country_code);
            Flag.sprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
            Avatar.sprite = await UserManager.Get.getAvatar(user.avatar);
            UserManager.Get.CurrentAvatarBytesString = Avatar.sprite;
        }

        public void UpdateHeaderInfo(User user)
        {
            UserManager.Get.UpdateUserMoneyCredit((user.money_credit).ToString());
            UserManager.Get.UpdateUserBubblesCredit(user.bubble_credit.ToString());
            Init(user);
        }

        public void OnMarketClick()
        {
           BottomMenuController.Get.SelectMarket();
        }
        #endregion
    }
}

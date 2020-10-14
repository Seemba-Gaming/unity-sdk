using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Timers;
using System.Linq;
public class EncartPlayerPresenter : MonoBehaviour
{
    public static Text username, money_credit, bubble_credit;
    public static Image avatar, PlayerAvatarDialog, flag;
    public Image EncartMoney;
    private static string usernameBackup;
    // Use this for initialization
    public void OnEnable()
    {
        Init();
    }
    // Update is called once per frame
    void Update()
    {
    }
    public static void Init()
    {
        if (!string.IsNullOrEmpty(UserManager.CurrentUsername))
        {
            usernameBackup = UserManager.CurrentUsername;
        }
        UserManager um = new UserManager();
        Texture2D txt = new Texture2D(1, 1);
        Sprite newSprite;
        foreach (var gameObj in FindObjectsOfType(typeof(Text)) as Text[])
        {
            if (gameObj.name == "Text_name")
            {
                if (!string.IsNullOrEmpty(UserManager.CurrentUsername))
                {
                    gameObj.text = UserManager.CurrentUsername;
                }
                else
                {
                    gameObj.text = PlayerPrefs.GetString("CurrentUsername");
                }
            }
        }
        try
        {
            GameObject.Find("Pro").transform.localScale = Vector3.zero;
            if (UserManager.CurrentUser.money_credit > 0)
            {
                GameObject.Find("Pro").transform.localScale = Vector3.one;
                try
                {
                    Image ProWallet = GameObject.Find("ProWallet").GetComponent<Image>();
                    ProWallet.transform.localScale = Vector3.one;
                }
                catch (NullReferenceException ex)
                {
                }
                try
                {
                    Image ProProfile = GameObject.Find("ProProfile").GetComponent<Image>();
                    ProProfile.transform.localScale = Vector3.one;
                }
                catch (NullReferenceException ex)
                {
                }
            }
        }
        catch (Exception ex) { }

        try
        {
            try
            {
                avatar = GameObject.Find("Avatar").GetComponent<Image>();
                avatar.sprite = UserManager.CurrentAvatarBytesString;
            }
            catch (NullReferenceException ex)
            {
            }
            //This for adapt bloc of money and bubbles for all kind of number
            Texture2D txt1 = new Texture2D(1, 1);
            Sprite newSprite1 = null;
            try
            {
                if (string.IsNullOrEmpty(PlayerPrefs.GetString("CurrentFlagBytesString")))
                {
                    PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.CurrentFlagBytesString);
                }
                Byte[] img1 = Convert.FromBase64String(UserManager.CurrentFlagBytesString);
                txt1.LoadImage(img1);
                newSprite1 = Sprite.Create(txt1 as Texture2D, new Rect(0f, 0f, txt1.width, txt1.height), Vector2.zero);
                try
                {
                    flag = GameObject.Find("Drapeau").GetComponent<Image>();
                    flag.sprite = newSprite1;
                    flag.transform.localScale = Vector3.one;
                }
                catch (NullReferenceException ex)
                {
                }
            }
            catch (ArgumentNullException ex)
            {
            }
            try
            {
                Image DrapeauProfile = GameObject.Find("DrapeauProfile").GetComponent<Image>();
                DrapeauProfile.sprite = newSprite1;
            }
            catch (NullReferenceException ex)
            {
            }
            try
            {
                Image DrapeauWallet = GameObject.Find("DrapeauWallet").GetComponent<Image>();
                DrapeauWallet.sprite = newSprite1;
            }
            catch (NullReferenceException ex)
            {
            }
        }
        catch (ArgumentNullException ex)
        {
        }
        try
        {
            username = GameObject.Find("Text_name").GetComponent<Text>();
            if (!PullToRefresh.pullActivated)
            {
                if (!string.IsNullOrEmpty(UserManager.CurrentUsername))
                {
                    username.text = UserManager.CurrentUsername;
                }
                else
                {
                    username.text = PlayerPrefs.GetString("CurrentUsername");
                }
            }
        }
        catch (NullReferenceException ex)
        {
        }
        try
        {
            bubble_credit = GameObject.Find("virtual_money").GetComponent<Text>();
            bubble_credit.text = UserManager.CurrentUser.bubble_credit.ToString();
        }
        catch (NullReferenceException ex) { }
        catch (ArgumentNullException ex) { }
        try
        {
            money_credit = GameObject.Find("solde_euro").GetComponent<Text>();
            money_credit.text = UserManager.CurrentUser.money_credit.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        }
        catch (NullReferenceException ex)
        {
        }
        catch (ArgumentNullException ex)
        {
        }

    }

}

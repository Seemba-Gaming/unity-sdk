using System;
using UnityEngine;
using UnityEngine.UI;
public class EncartPlayerPresenter : MonoBehaviour
{
    public static Text PlayerUsername, PlayerMoney, PlayerWater;
    public static Image PlayerAvatar, PlayerAvatarDialog, PlayerFlag;
    public Image avatar;
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
            if (float.Parse(UserManager.CurrentMoney) > 0)
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
        catch (NullReferenceException ex)
        {
        }
        catch (ArgumentNullException ex)
        {
        }
        try
        {
            try
            {
                PlayerAvatar = GameObject.Find("Avatar").GetComponent<Image>();
                PlayerAvatar.sprite = UserManager.CurrentAvatarBytesString;
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
                    PlayerFlag = GameObject.Find("Drapeau").GetComponent<Image>();
                    PlayerFlag.sprite = newSprite1;
                    PlayerFlag.transform.localScale = Vector3.one;
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
            PlayerUsername = GameObject.Find("Text_name").GetComponent<Text>();
            if (!PullToRefresh.pullActivated)
            {
                if (!string.IsNullOrEmpty(UserManager.CurrentUsername))
                {
                    PlayerUsername.text = UserManager.CurrentUsername;
                }
                else
                {
                    PlayerUsername.text = PlayerPrefs.GetString("CurrentUsername");
                }
            }
        }
        catch (NullReferenceException ex)
        {
        }
        try
        {
            PlayerWater = GameObject.Find("virtual_money").GetComponent<Text>();
            PlayerWater.text = int.Parse(UserManager.CurrentWater).ToString();
        }
        catch (NullReferenceException ex) { }
        catch (ArgumentNullException ex) { }
        try
        {
            PlayerMoney = GameObject.Find("solde_euro").GetComponent<Text>();
            if (float.Parse(UserManager.CurrentMoney) == 0)
            {
                PlayerMoney.text = "0.00 " + CurrencyManager.CURRENT_CURRENCY;
            }
            else PlayerMoney.text = float.Parse(UserManager.CurrentMoney).ToString("N2") + " " + CurrencyManager.CURRENT_CURRENCY;
        }
        catch (NullReferenceException ex)
        {
        }
        catch (ArgumentNullException ex)
        {
        }
        try
        {
            GameObject.Find("PanelLoader").SetActive(false);
        }
        catch (NullReferenceException ex)
        {
        }
    }
}

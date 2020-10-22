using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Globalization;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class ProfileViewPresenter : MonoBehaviour
{
    public static string PlayerId;
    public static Sprite Avatar;
    public Image avatar, pro, drapeau;
    public Text username, nbGameWon, nbGameWonInARow;
    public Button changeAvatar;
    public GameObject Loading;
    public GameObject verified, pending, unverified;
    UserManager um = new UserManager();

    private static ProfileViewPresenter _Instance = null;

    // Use this for initialization

    private ProfileViewPresenter()
    {
    }
    public static ProfileViewPresenter Instance
    {
        get
        {

            return _Instance;
        }
    }

   async void Start()
    {
        _Instance = this;

        ProfilLastResultListController.profileSceneOpened = true;
        Texture2D txtAvatar = new Texture2D(1, 1);
        Sprite newSpriteAvatar;
        Texture2D txtDrapeau = new Texture2D(1, 1);
        Sprite newSpriteDrapeau;
        WithdrawManager wm = new WithdrawManager();
        UnityThreading.ActionThread thread;
        Loading.SetActive(true);
        string token = um.getCurrentSessionToken();
        string userId = um.getCurrentUserId();
        JSONNode account = "";
        if (PlayerId == userId)
        {
            account = await wm.accountVerificationStatus(token);
        }
        UnityThreadHelper.CreateThread(() =>
        {
            
            User player = um.getUser(PlayerId, token);
            string Vectoires = player.victories_count.ToString();
            //Debug.Log("Vectoires " +Vectoires);
            string SerieVectoires = player.victories_streak.ToString();
            //Debug.Log("SerieVectoires " + SerieVectoires);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                Loading.SetActive(false);
                if (PlayerId != userId)
                {
                    changeAvatar.interactable = false;
                    avatar.sprite = Avatar;
                }
                else
                {
                    avatar.sprite = UserManager.CurrentAvatarBytesString;
                     if (account["verification_status"].Value.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING))
                     {
                         pending.SetActive(true);
                     }
                    if (account["verification_status"].Value.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED))
                    {
                        verified.SetActive(true);
                    }
                    else unverified.SetActive(true);
                }
                //Set User Name
                username.text = player.username;
                //Set User pro label
                try
                {
                    if (player.money_credit > 0)
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
                    if (player.money_credit > 0)
                    {
                        pro.gameObject.SetActive(true);
                    }
                    else
                    {
                        pro.gameObject.SetActive(false);
                    }
                }
                //set User Drapeau
                Byte[] img1 = Convert.FromBase64String(um.GetFlagByte(player.country_code));
                txtDrapeau.LoadImage(img1);
                newSpriteDrapeau = Sprite.Create(txtDrapeau as Texture2D, new Rect(0f, 0f, txtDrapeau.width, txtDrapeau.height), Vector2.zero);
                drapeau.sprite = newSpriteDrapeau;
                //set Games Won
                nbGameWon.text = Vectoires;
                //set nb Game Won In a Row
                nbGameWonInARow.text = SerieVectoires;
            });
        });
    }
    public async void loadNewAvatar(Texture2D texture)
    {
        byte[] bytes;
        bytes = texture.EncodeToPNG();
        await uploadAvatar(bytes);
    }

    async Task uploadAvatar(byte[] bytes)
    {
       var res=await ImagesManager.FixImage(bytes);
        
        if (!string.IsNullOrEmpty(res) && !res.Equals("error"))
        {
            ImagesManager.AvatarURL = res;

            if (!string.IsNullOrEmpty(ImagesManager.AvatarURL))
            {
                var www = UnityWebRequestTexture.GetTexture(ImagesManager.AvatarURL);
                await www.SendWebRequest();
                cropAndShowAvatar(((DownloadHandlerTexture)www.downloadHandler).texture);
            }
        }
    }
    void cropAndShowAvatar(Texture2D texture)
    {
        Texture2D RoundTxt = ImagesManager.RoundCrop(texture);
        Sprite newSprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
        //Create Sprite and change Profile Avatar
        try
        {
            GameObject.Find("_Avatar").GetComponent<Image>().sprite = newSprite;
        }
        catch (NullReferenceException ex)
        {
        }
        GameObject.Find("Avatar").GetComponent<Image>().sprite = newSprite;
        //Update Current user Avatar in Views
        updateUserAvatar(newSprite);

    }
    void updateUserAvatar(Sprite sprite)
    {
        UserManager.CurrentAvatarBytesString = sprite;
        if (SceneManager.GetActiveScene().name != "Signup")
        {
            string userId = um.getCurrentUserId();
            string token = um.getCurrentSessionToken();
            UnityThreadHelper.CreateThread(() =>
            {
                //Update Avatar in DATABASE
                string[] attrib = { "avatar" };
                string[] value = { ImagesManager.AvatarURL };
                um.UpdateUserByField(userId, token, attrib, value);
            });
        }
    }

}

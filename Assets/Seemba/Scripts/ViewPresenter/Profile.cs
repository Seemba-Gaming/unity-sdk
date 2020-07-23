using System;
using UnityEngine;
using UnityEngine.UI;
public class Profile : MonoBehaviour
{
    public static string PlayerId;
    public static Sprite Avatar;
    public Image avatar, pro, drapeau;
    public Text username, nbGameWon, nbGameWonInARow;
    public Button changeAvatar;
    public GameObject Loading;
    public GameObject verified, pending, unverified;
    UserManager um = new UserManager();
    // Use this for initialization
    void Start()
    {
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
        string accountStatus = "";
        thread = UnityThreadHelper.CreateThread(() =>
        {
            if (PlayerId == userId)
            {
                accountStatus = wm.accountVerificationStatus(token);
            }
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
                    if (accountStatus == WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING)
                    {
                        pending.SetActive(true);
                    }
                    else if (accountStatus == WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED)
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
    // Update is called once per frame
    void Update()
    {
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
public class OpponentFound : MonoBehaviour
{
    public static string adversaireName;
    public Text opponent_username;
    public static string Avatar, AdvCountryCode;
    // Use this for initialization
    ChallengeManager challengeManager;
    public Image opponent_avatar, opponent_flag;
    public GameObject loaderPending, PanelLookingForPlayer, PanelPlayerFound, OpponentName;
    public GameObject Versus_container;
    public Animator Versus_background;
    public void Start()
    {
        InvokeRepeating("init", 0f, 1f);
    }
    public void init()
    {
        UserManager um = new UserManager();
        UserManager manager = new UserManager();

        UnityThreadHelper.CreateThread(() =>
        {
            if (EventsController.advFound == true)
            {
                Byte[] lnByte = manager.getAvatar(Avatar);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    CancelInvoke();
                    Texture2D txt = new Texture2D(1, 1);
                    Sprite newSprite;
                    PanelLookingForPlayer.SetActive(false);
                    PanelPlayerFound.transform.localScale = Vector3.one;
                    challengeManager = new ChallengeManager();
                    opponent_username.text = adversaireName;
                    Byte[] img;
                    opponent_avatar.sprite = ImagesManager.getSpriteFromBytes(lnByte);
                    try
                    {
                        img = Convert.FromBase64String(manager.GetFlagByte(AdvCountryCode));
                        txt.LoadImage(img);
                        newSprite = Sprite.Create(txt as Texture2D, new Rect(0f, 0f, txt.width, txt.height), Vector2.zero);
                        opponent_flag.sprite = newSprite;
                    }
                    catch (NullReferenceException ex)
                    {

                    }
                    Versus_background.SetBool("StopBG", true);
                    Versus_container.SetActive(true);
                });
            }
        });
    }
    // Update is called once per frame
    void Update()
    {
    }
}

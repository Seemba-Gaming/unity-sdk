using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    #pragma warning disable 649
    public class BottomBarTranslationController : MonoBehaviour
    {
        [SerializeField]
        private Text home, have_fun, win_money, setting, leaderboard;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager._instance.scene = "BottomBar";
            home.text = TranslationManager._instance.Get("home") != string.Empty ? TranslationManager._instance.Get("home") : home.text;
            have_fun.text = TranslationManager._instance.Get("have_fun") != string.Empty ? TranslationManager._instance.Get("have_fun") : have_fun.text;
           // win_money.text = TranslationManager._instance.Get("win_money") != string.Empty ? TranslationManager._instance.Get("win_money") : win_money.text;
            win_money.text = TranslationManager._instance.Get("crowns") != string.Empty ? TranslationManager._instance.Get("crowns") : win_money.text;
            setting.text = TranslationManager._instance.Get("settings") != string.Empty ? TranslationManager._instance.Get("settings") : setting.text;
            if(leaderboard != null)
                leaderboard.text = TranslationManager._instance.Get("leaderboard") != string.Empty ? TranslationManager._instance.Get("leaderboard") : leaderboard.text;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

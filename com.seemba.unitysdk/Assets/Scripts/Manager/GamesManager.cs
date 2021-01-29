using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class GameInfo
    {
        public GamesData game;
        public GameChallengesInfo fees;
        public GameChallengesInfo gain;
    }

    [CLSCompliant(false)]
    public class GamesData
    {
        public string app_store_link;
        public bool completed;
        public bool deleted;
        public string _id;
        public string[] platforms;
        public string[] brackets;
        public string[] tournaments;
        public string name;
        public string editor;
        public string team;
        public string appstore_id;
        public string bundle_id;
        public string createdAt;
        public string description;
        public string store_link;
        public string icon;
        public string screenshot;
        public string background_image;
        public string gcm_api_key;
        public string status;
        public string orientation;
        public string engine;
        public string android_name;
        public string android_version;
        public string ios_name;
        public string ios_version;
        public string __v;
        public string score_mode;
        public string updatedAt;
    }
    public class GameChallengesInfo
    {
        public DuelsPrice duels;
        public TournamentsPrice tournament;
    }

    public class DuelsPrice
    {
        public ChallengePrice Confident;
        public ChallengePrice Champion;
        public ChallengePrice Legend;
    }

    public class TournamentsPrice
    {
        public ChallengePrice Amateur;
        public ChallengePrice Novice;
        public ChallengePrice Confirmed;
    }

    public class ChallengePrice
    {
        public float cash;
        public float bubbles;
    }
    [CLSCompliant(false)]
    public class GamesManager : MonoBehaviour
    {
        #region Static
        public static GamesManager Get { get { return sInstance; } }

        private static GamesManager sInstance;
        #endregion

        //Set The Game Name*
        public static string GAME_NAME;
        //
        public static string GAME_ANDROID_URL = "https://play.google.com/store/apps/details?id=com.seemba.desertdashmexico";
        public static string GAME_IOS_URL;
        //Set Scene name of game , Entry point to the game
        public static string GAME_SCENE_NAME;
        public static string GAME_ORIENTATION;
        public static bool GAME_SCENE_SEPERATED = false;
        public static string EDITOR_ID;
        //The game level number used for your matchmaking. Otherwise, keep it null.
        public static int? GAME_LEVEL = null;

        //Set The Game Id shown in your Dashboard ,you can't start without setting the correct id
        internal static string GAME_ID; //staging

        public static string ICON_URL;
        public static string BACKGROUND_IMAGE_URL;

        public static bool? backgroundSaved = null;
        public static bool? iconSaved = null;
        public static Sprite CurrentIcon;
        public bool AutoPlayActivated;
        private void Awake()
        {
            sInstance = this;
        }
        public static async Task<bool> SaveIcon(string url)
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("IconURL")) || !PlayerPrefs.GetString("IconURL").Equals(url))
            {
                var www = UnityWebRequestTexture.GetTexture(url);
                await www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    return false;
                }
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                byte[] bytes;
                bytes = texture.EncodeToPNG();
                File.WriteAllBytes(Application.persistentDataPath + '/' + "icon.png", bytes);
                PlayerPrefs.SetString("IconURL", url);
                return true;
            }
            else
            {
                return true;
            }
        }

        public static void LoadIcon()
        {
            byte[] bytes;
            bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + '/' + "icon.png");
            Texture2D txt = new Texture2D(1, 1);
            txt.LoadImage(bytes);
            txt.Apply();
            CurrentIcon = Sprite.Create(txt, new Rect(0, 0, txt.width, txt.height), new Vector2(0, 0));
        }

        public async Task<GameInfo> getGamebyId(string gameId)
        {
            var url = Endpoint.classesURL + "/games/" + gameId;
            var req = await SeembaWebRequest.Get.HttpsGetJSON<GameInfo>(url);
            if (req != null)
            {
                ChallengeManager.Get.InitFees(req.fees);
                ChallengeManager.Get.InitGains(req.gain);
                TournamentManager.Get.InitGains(req.gain);
                TournamentManager.Get.InitGains(req.gain);
                var duels = req.game.brackets;
                foreach (string duel_type in duels)
                {
                    TournamentManager.AVALAIBLE_TOURNAMENTS.Add(duel_type);
                }
                var tournaments = req.game.tournaments;

                foreach (string tournament_type in tournaments)
                {
                    ChallengeManager.AVALAIBLE_CHALLENGE.Add(tournament_type);
                }

                if (!string.IsNullOrEmpty(req.game.background_image))
                {
                    BACKGROUND_IMAGE_URL = req.game.background_image;
                }
                else
                {
                    Debug.LogError("Please add background-image for your game in the dashboard");
                }

                if (!string.IsNullOrEmpty(req.game.icon))
                {
                    ICON_URL = req.game.icon;
                }
                else
                {
                    Debug.LogError("Please add icon for your game in the dashboard");
                }

                return req;
            }
            else
            {
                return null;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    #pragma warning disable 649
    public class ReplayChallengeTranslationController : MonoBehaviour
    {
        [SerializeField]
        private Text sorry,
          bug,
          replay_now,
          game_id,
          date,
          prize,
          your_opponent,
          is_waiting,
          play_now;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager._instance.scene = "ReplayChallenge";
            sorry.text = TranslationManager._instance.Get("sorry") != string.Empty ? TranslationManager._instance.Get("sorry") : sorry.text;
            bug.text = TranslationManager._instance.Get("challenge_not_finished") != string.Empty ? TranslationManager._instance.Get("challenge_not_finished") : bug.text;
            replay_now.text = TranslationManager._instance.Get("replay_now") != string.Empty ? TranslationManager._instance.Get("replay_now") : replay_now.text;
            game_id.text = TranslationManager._instance.Get("game_id") != string.Empty ? TranslationManager._instance.Get("game_id") : game_id.text;
            date.text = TranslationManager._instance.Get("date") != string.Empty ? TranslationManager._instance.Get("date") : date.text;
            your_opponent.text = TranslationManager._instance.Get("your_opponent") != string.Empty ? TranslationManager._instance.Get("your_opponent") : your_opponent.text;
            is_waiting.text = TranslationManager._instance.Get("is_waiting") != string.Empty ? TranslationManager._instance.Get("is_waiting") : is_waiting.text;
            play_now.text = TranslationManager._instance.Get("play_now") != string.Empty ? TranslationManager._instance.Get("play_now") : play_now.text;
            TranslationManager._instance.scene = "Home";
            prize.text = TranslationManager._instance.Get("gain") != string.Empty ? TranslationManager._instance.Get("gain") : prize.text;
        }
    }
}

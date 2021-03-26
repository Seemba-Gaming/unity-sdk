using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
#pragma warning disable 0649
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
            TranslationManager.scene = "ReplayChallenge";
            sorry.text = TranslationManager.Get("sorry") != string.Empty ? TranslationManager.Get("sorry") : sorry.text;
            bug.text = TranslationManager.Get("bug") != string.Empty ? TranslationManager.Get("bug") : bug.text;
            replay_now.text = TranslationManager.Get("replay_now") != string.Empty ? TranslationManager.Get("replay_now") : replay_now.text;
            game_id.text = TranslationManager.Get("game_id") != string.Empty ? TranslationManager.Get("game_id") : game_id.text;
            date.text = TranslationManager.Get("date") != string.Empty ? TranslationManager.Get("date") : date.text;
            your_opponent.text = TranslationManager.Get("your_opponent") != string.Empty ? TranslationManager.Get("your_opponent") : your_opponent.text;
            is_waiting.text = TranslationManager.Get("is_waiting") != string.Empty ? TranslationManager.Get("is_waiting") : is_waiting.text;
            play_now.text = TranslationManager.Get("play_now") != string.Empty ? TranslationManager.Get("play_now") : play_now.text;
            TranslationManager.scene = "Home";
            prize.text = TranslationManager.Get("gain") != string.Empty ? TranslationManager.Get("gain") : prize.text;
        }
    }
}

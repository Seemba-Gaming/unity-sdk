using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class OnGoingGameListController : MonoBehaviour
    {
        public GameObject ContentPanel, ContentOngoing;
        public GameObject ListItemPrefab;
        public GameObject tournamentPrefab;
        public GameObject NoResults;
        public Button SeeMoreResult;
        public int nbElement = 0;
        string token;
        public Sprite[] spriteArray;
        GenericChallenge[] mChallengesList;
        private bool initialized = false;
        int page = 1;
        int pageSize = 4;
        async void OnEnable()
        {
            if (initialized == false)
            {
                nbElement = 0;
                page = 1;
                initialized = true;
                HomeController.NoOngoing = false;
                StartCoroutine(removeDuplicationCoroutine());
                token = UserManager.Get.getCurrentSessionToken();
                if (token != null)
                {
                    mChallengesList = await ChallengeManager.Get.GetOnGoingChallenges(1, pageSize);
                    if (mChallengesList.Length > 0)
                    {
                        ContentPanel.SetActive(true);
                        ContentOngoing.SetActive(true);
                        SeeMoreResult.gameObject.SetActive(true);
                        NoResults.SetActive(false);
                        DisplayOnGoingChallenges(mChallengesList);
                    }
                    else
                    {
                        //ContentOngoing.SetActive(false);
                        //ContentPanel.SetActive(false);
                        SeeMoreResult.gameObject.SetActive(false);
                        NoResults.SetActive(true);
                    }
                    SeeMoreResult.onClick.RemoveAllListeners();
                    SeeMoreResult.onClick.AddListener(async () =>
                    {
                        page++;
                        mChallengesList = await ChallengeManager.Get.GetOnGoingChallenges(page, pageSize);
                        if (mChallengesList.Length > 0)
                        {
                            DisplayOnGoingChallenges(mChallengesList);
                        }
                        else
                        {
                            SeeMoreResult.gameObject.SetActive(false);
                        }
                    });
                    PullToRefresh.ongoingfinished = true;
                }
                else
                {
                    Debug.LogWarning(initialized);
                }
            }
        }
        void OnDisable()
        {
            initialized = false;
            foreach (Transform child in ContentPanel.transform)
            {
                if (child.name != "PARTIES EN COURS")
                {
                    Destroy(child.gameObject);
                }
            }
            nbElement = 0;
            CancelInvoke("removeDuplication");
        }
        void DisplayOnGoingChallenges(GenericChallenge[] list)
        {
            nbElement += mChallengesList.Length;

            if (mChallengesList.Length == 4)
            {
                SeeMoreResult.gameObject.SetActive(true);
            }
            else
            {
                SeeMoreResult.gameObject.SetActive(false);
            }

            foreach (GenericChallenge challenge in list)
            {
                if ((challenge.matched_user_1 != null && challenge.matched_user_1._id == UserManager.Get.CurrentUser._id && challenge.user_1_score == null) || (challenge.matched_user_2 != null && challenge.matched_user_2._id == UserManager.Get.CurrentUser._id && challenge.user_2_score == null))
                {
                    //draw in a tournament
                    if(challenge.tournament_id != null && challenge.users_old_scores.Length > 0 && challenge.users_old_scores[0].user_1_score == challenge.users_old_scores[0].user_1_score)
                    {
                        Debug.LogWarning("Draw in a tournament");
                        InitOnGoingTournament(challenge);
                        //if(!PlayerPrefs.HasKey("ShowPopup") || (PlayerPrefs.HasKey("ShowPopup") && PlayerPrefs.GetInt("ShowPopup") == 1))
                        if(!PopupsViewPresenter.NeverShowAgain)
                        {
                            TranslationManager._instance.scene = "Popups";
                            var _params = new object[] 
                            {
                                TranslationManager._instance.Get("tournament"),
                                TranslationManager._instance.Get("draw"),
                                challenge.matched_user_1.avatar,
                                challenge.matched_user_2.avatar,
                                challenge.users_old_scores[0].user_1_score,
                                challenge.users_old_scores[0].user_2_score,
                                TranslationManager._instance.Get("you_have"),
                                TranslationManager._instance.Get("play_or_lose"),
                                TranslationManager._instance.Get("play_now"),
                                TranslationManager._instance.Get("play_later"),
                                challenge.matched_user_1.username,
                                challenge.matched_user_2.username,
                                challenge.updatedAt,
                                challenge.tournament_id
                            };
                            PopupManager.Get.PopupController.ShowPopup(PopupType.TOURNAMENT_DRAW, _params);
                        }

                        SeembaAnalyticsManager.Get.SendTournamentEvent("Tournament Draw", challenge.tournament_id, challenge.users_old_scores[0].user_1_score);
                    }
                    else
                    {
                        if(challenge.tournament_id == null)
                        {
                            ChallengeManager.CurrentChallengeId = challenge._id;
                            ReplayChallengePresenter.ChallengeToReplay = challenge;
                            SeembaAnalyticsManager.Get.SendDuelInfoEvent("Bug Challenge", ChallengeManager.Get.GetChallengeFee(float.Parse(challenge.gain), challenge.gain_type), float.Parse(challenge.gain), challenge.gain_type);
                            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ReplayChallenge.gameObject,false,false);
                        }
                    }
                }
                else
                {
                    if (challenge.tournament_id != null)
                    {
                        InitOnGoingTournament(challenge);
                    }
                    else if(challenge._id != null)
                    {
                        InitOnGoingChallenge(challenge);
                    }
                    else
                    {
                        Debug.LogWarning("id null");
                    }
                }
            }
        }
        private void InitOnGoingTournament(GenericChallenge challenge)
        {
            GameObject newItem = Instantiate(tournamentPrefab) as GameObject;
            OnGoingTournamentListItemController controller = newItem.GetComponent<OnGoingTournamentListItemController>();
            string date = challenge.createdAt.Substring(0, challenge.createdAt.IndexOf("T"));
            string hour = challenge.createdAt.Substring(challenge.createdAt.IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
            controller.Date.text = date + " " + HomeTranslationController.AT + " " + hour;
            TranslationManager._instance.scene = "Home";
            controller.status.text = TranslationManager._instance.Get("pending");
            controller.tournamentId.text = challenge.tournament_id;
            controller.gainType = challenge.tournament.gain_type;
            controller.CreatedAt = challenge.tournament.createdAt;
            controller.gain = challenge.tournament.gain;
            SetControllerTournamentTitle(challenge, controller);
            controller.GoToBracket.onClick.AddListener(() =>
            {
                TournamentController.setCurrentTournamentID(challenge.tournament_id);
                ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
            });
            newItem.transform.SetParent(ContentPanel.transform);
            RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
            myLayoutElement.transform.localScale = Vector3.one;
        }
        private void InitOnGoingChallenge(GenericChallenge challenge)
        {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            OnGoingGameListItemController controller = newItem.GetComponent<OnGoingGameListItemController>();
            controller.challengeResultId.text = challenge._id;
            controller.challengeId.text = challenge._id;
            SetControllerTitle(challenge, controller);
            if (challenge.status.Equals("results pending") || challenge.status.Equals("pending") || challenge.status.Equals("on going"))
            {
                TranslationManager._instance.scene = "Home";
                controller.pending_text.text = TranslationManager._instance.Get("pending");
                controller.SeeResult.transform.localScale = Vector3.zero;
                controller.Result.gameObject.SetActive(true);
                controller.Result.onClick.AddListener(async () =>
                {
                    LoaderManager.Get.LoaderController.ShowLoader(null);
                    ChallengeManager.CurrentChallengeId = challenge._id;
                    Challenge mCurrentChallenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
                    ChallengeManager.CurrentChallenge = mCurrentChallenge;
                    ChallengeManager.Get.ShowResult();
                    LoaderManager.Get.LoaderController.HideLoader();

                });
                controller.SeeResult.gameObject.SetActive(false);
            }
            else if (challenge.status.Equals("see results for user 1") || challenge.status.Equals("see results for user 2"))
            {
                controller.pending_text.text = HomeTranslationController.GAME_FINISHED;
                controller.SeeResult.transform.localScale = Vector3.one;
                controller.SeeResult.gameObject.SetActive(true);
                controller.SeeResult.onClick.AddListener(async () =>
                {
                    ChallengeManager.CurrentChallengeId = challenge._id;
                    LoaderManager.Get.LoaderController.ShowLoader(null);

                    GenericChallenge mUpdatedChallenge = await ChallengeManager.Get.UpdateChallengeStatusToFinishedAsync(token, ChallengeManager.CurrentChallengeId);
                    Challenge Selectedchallenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);

                    ChallengeManager.CurrentChallenge = Selectedchallenge;
                    ChallengeManager.Get.ShowResult();
                    LoaderManager.Get.LoaderController.HideLoader();

                });
                controller.Result.gameObject.SetActive(false);
            }
            string date = challenge.createdAt.ToString().Substring(0, challenge.createdAt.ToString().IndexOf("T"));
            string hour = challenge.createdAt.ToString().Substring(challenge.createdAt.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
            controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
            newItem.transform.SetParent(ContentPanel.transform);
            RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
            myLayoutElement.transform.localScale = Vector3.one;
        }
        void SetControllerTournamentTitle(GenericChallenge item, OnGoingTournamentListItemController controller)
        {
            if (item.gain_type == TournamentManager.GAIN_TYPE_CASH)
            {
                if (float.Parse(item.gain) == TournamentManager.WIN_BRACKET_CASH_AMATEUR)
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_CASH_AMATEUR * 100 + " <sprite=1>";
                }
                else if (float.Parse(item.gain) == TournamentManager.WIN_BRACKET_CASH_NOVICE)
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_CASH_NOVICE * 100 + " <sprite=1>";
                }
                else if (float.Parse(item.gain) == TournamentManager.WIN_BRACKET_CASH_CONFIRMED)
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_CASH_CONFIRMED * 100 + " <sprite=1>";
                }
            }
            else
            {
                if (float.Parse(item.gain) == TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR)
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR + " <sprite=0>";
                }
                else if (float.Parse(item.gain) == TournamentManager.WIN_BRACKET_BUBBLE_NOVICE)
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_BUBBLE_NOVICE + " <sprite=0>";
                }
                else if (float.Parse(item.gain) == TournamentManager.WIN_BRACKET_BUBBLE_CONFIRMED)
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_BUBBLE_CONFIRMED + " <sprite=0>";
                }
            }
        }
        void SetControllerTitle(GenericChallenge item, OnGoingGameListItemController controller)
        {
            if (item.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
            {
                if (item.gain == ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString())
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_CASH_CONFIDENT * 100 + " <sprite=1>";
                }
                else if (item.gain == ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString())
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_CASH_CHAMPION * 100 + " <sprite=1>";
                }
                else if (item.gain == ChallengeManager.WIN_1V1_CASH_LEGEND.ToString())
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_CASH_LEGEND * 100 + " <sprite=1>";
                }
            }
            else
            {
                if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString())
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT + " <sprite=0>";
                }
                else if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString())
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_CHAMPION + " <sprite=0>";
                }
                else if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString())
                {
                    controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_LEGEND + " <sprite=0>";
                }
            }
        }
        public IEnumerator removeDuplicationCoroutine()
        {
            while(ContentPanel.transform.childCount > nbElement)
            {
                removeDuplication();
                yield return new WaitForSeconds(0.2f);
            }
        }
        void removeDuplication()
        {
            if (ContentPanel.transform.childCount > nbElement)
            {
                CancelInvoke();
                int count = 0;
                foreach (Transform child in ContentPanel.transform)
                {
                    if (count >= nbElement)
                    {
                        Destroy(child.gameObject);
                    }
                    count++;
                }
            }
        }
    }
}
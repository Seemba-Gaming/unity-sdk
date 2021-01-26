using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class ProfilLastResultListController : MonoBehaviour
    {
        public GameObject ContentPanel;
        public GameObject ItemPrefab;
        private ArrayList Items, lastResultItem;
        private string UserId, token;
        public static bool profileSceneOpened;

        private void OnDisable()
        {
            foreach (Transform child in ContentPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnEnable()
        {
            try
            {
                Items = new ArrayList();
                if (profileSceneOpened == true)
                {
                    profileSceneOpened = false;
                    UserId = ViewsEvents.Get.Profile.PlayerId;
                }
                else
                {
                    UserId = UserManager.Get.getCurrentUserId();
                }
                UnityThreading.ActionThread thread;
                thread = UnityThreadHelper.CreateThread(async () =>
                {
                    lastResultItem = await ChallengeManager.Get.listChallenges();
                    foreach (Challenge item in lastResultItem)
                    {
                        if (item.status == ChallengeManager.CHALLENGE_STATUS_FINISHED || (item.status == ChallengeManager.CHALLENGE_STATUS_SEE_RESULT_FOR_USER1 && item.matched_user_2._id == UserId) || (item.status == ChallengeManager.CHALLENGE_STATUS_SEE_RESULT_FOR_USER2 && item.matched_user_1._id == UserId))
                        {
                            Items.Add(item);
                        }
                    }
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        if (Items != null)
                        {
                            Items.Reverse();
                            foreach (Challenge item in Items)
                            {
                                if (item.status != ChallengeManager.CHALLENGE_STATUS_RESULT_PENDING)
                                {
                                    GameObject newItem = null;
                                    try
                                    {
                                        newItem = Instantiate(ItemPrefab);
                                        if (item.user_1_score == item.user_2_score)
                                        {
                                            newItem.transform.GetChild(4).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                        }
                                        if (UserId == item.matched_user_1._id)
                                        {
                                            string UserToken = UserManager.Get.getCurrentSessionToken();
                                            if (item.user_1_score > item.user_2_score)
                                            {
                                            //Win
                                            newItem.transform.GetChild(2).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                            }
                                            else if (item.user_1_score < item.user_2_score)
                                            {
                                            //Lose
                                            newItem.transform.GetChild(3).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                            }
                                        }
                                        else
                                        {
                                            string UserToken = UserManager.Get.getCurrentSessionToken();
                                            if (item.user_1_score > item.user_2_score)
                                            {
                                            //LOSE
                                            newItem.transform.GetChild(3).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                            }
                                            else if (item.user_1_score < item.user_2_score)
                                            {
                                            //Win
                                            newItem.transform.GetChild(2).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                            }
                                        }

                                        newItem.transform.SetParent(ContentPanel.transform);
                                        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                                        myLayoutElement.transform.localScale = Vector3.one;

                                    }
                                    catch (FormatException)
                                    {
                                        Destroy(newItem);
                                    }
                                }
                            }
                        }
                    });
                });
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}

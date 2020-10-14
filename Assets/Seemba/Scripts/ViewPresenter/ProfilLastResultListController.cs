using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class ProfilLastResultListController : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ItemPrefab;
    ArrayList Items, lastResultItem;
    ChallengeManager challengeManager = new ChallengeManager();
    UserManager userManager = new UserManager();
    string UserId, token;
    public static bool profileSceneOpened;
    // Use this for initialization
    void OnEnable()
    {
        try
        {
            token = userManager.getCurrentSessionToken();
            Items = new ArrayList();
            if (profileSceneOpened == true)
            {
                profileSceneOpened = false;
                UserId = ProfileViewPresenter.PlayerId;
            }
            else
            {
                UserId = userManager.getCurrentUserId();
            }
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                lastResultItem = challengeManager.getFinishedChallenges(token);
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
                                    newItem = Instantiate(ItemPrefab) as GameObject;
                                    if (item.user_1_score == item.user_2_score)
                                    {
                                        //DRAW
                                        newItem.transform.GetChild(4).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                    }
                                    else if (item.winner_user.Equals(UserId))
                                    {
                                        //Win
                                        newItem.transform.GetChild(2).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                    }
                                    else
                                    {
                                        //Lose
                                        newItem.transform.GetChild(3).gameObject.GetComponent<Image>().transform.localScale = Vector3.one;
                                    }
                                    newItem.transform.parent = ContentPanel.transform;
                                    RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                                    myLayoutElement.transform.localScale = Vector3.one;
                                }
                                catch (FormatException e)
                                {
                                    Destroy(newItem);
                                }
                            }
                        }
                    }
                });
            });
        }
        catch (NullReferenceException ex)
        {
        }
    }

}

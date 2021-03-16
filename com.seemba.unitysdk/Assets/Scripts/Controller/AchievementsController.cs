using SimpleJSON;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class AchievementItem
    {
        public string _id;
        public Image image;
        public string name;
        public string description;
        public int amount;
        public int __v;
    }

    [CLSCompliant(false)]
    public class AchievementsController : MonoBehaviour
    {

        #region Script Parameters
        public Toggle               Won;
        public Toggle               ToBeWon;
        public Transform            ItemsContainer;
        public GameObject           AchievementPrefab;
        #endregion

        #region Fields
        private bool mCanClickWon = true;
        private bool mCanClickToBeWon = false;
        private AchievementItem[] mMyAchievements;
        private AchievementItem[] mAllAchievements;
        private AchievementItem[] mToBeWonAchievements;
        #endregion

        #region Unity Methods

        private void Start()
        {
            OnClickWon(!Won.isOn);
            OnClickToBeWonAsync(!ToBeWon.isOn);
        }

        private void OnEnable()
        {
            if(Won.isOn)
            {
                OnClickWon(true);
            }
            else
            {
                OnClickToBeWonAsync(true);
            }
        }
        #endregion

        #region Methods
        public async void OnClickWon(bool selected)
        {
            if (selected && mCanClickWon) 
            {
                Won.GetComponent<RectTransform>().localScale = Vector3.one;
                ToBeWon.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                mCanClickToBeWon = true;
                mCanClickWon = false;
                await GetMyAchivements();
                FillAchievements(mMyAchievements);
            }
        }
        public async void OnClickToBeWonAsync(bool selected)
        {
            if (selected && mCanClickToBeWon)
            {
                ToBeWon.GetComponent<RectTransform>().localScale = Vector3.one;
                Won.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                mCanClickWon = true;
                mCanClickToBeWon = false;
                await GetAllAchivements();
                FillAchievements(mAllAchievements);
            }
        }

        public async Task<bool> GetAllAchivements()
        {
            string url = Endpoint.classesURL + "/gamifications/achievements/all";
            var seembaResponse = await SeembaWebRequest.Get.HttpsGetJSON<AchievementItem[]>(url);
            mAllAchievements = seembaResponse;
            return true;
        }

        private void FillAchievements(AchievementItem[] items)
        {
            foreach (AchievementItem item in items)
            {
                GameObject AchievementItem = Instantiate(AchievementPrefab, ItemsContainer);
                AchievementItem.GetComponent<RectTransform>().localScale = Vector3.one;
                AchievementItem.GetComponent<AchievementsItemController>().Init(item);
            }
        }

        public async Task<bool> GetMyAchivements()
        {
            string url = Endpoint.classesURL + "/gamifications/achievements/done";
            var seembaResponse = await SeembaWebRequest.Get.HttpsGetJSON<AchievementItem[]>(url);
            mMyAchievements = seembaResponse;
            return true;
        }

        #endregion
    }
}

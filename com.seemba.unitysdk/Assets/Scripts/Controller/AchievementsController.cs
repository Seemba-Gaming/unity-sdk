using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class AchievementItem
    {
        public string name;
        public string description;
        public bool done;
        public int current_amount;
        public int total_amount;
    }
    public class AchievemenstsList
    {
        public string user_id;
        public AchievementItem[] achievements;
    }

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
                //Won.GetComponent<RectTransform>().localScale = Vector3.one;
                //ToBeWon.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                Won.targetGraphic.GetComponent<Image>().color = new Color(255/255f, 139/255f, 87/255f, 255/255f);
                ToBeWon.targetGraphic.GetComponent<Image>().color = new Color(255/255f, 139/255f, 87/255f, 150/255f);
                mCanClickToBeWon = true;
                mCanClickWon = false;
                await GetMyAchivements();
                FillAchievements(mMyAchievements, true);
            }
        }
        public async void OnClickToBeWonAsync(bool selected)
        {
            if (selected && mCanClickToBeWon)
            {
                //ToBeWon.GetComponent<RectTransform>().localScale = Vector3.one;
                //Won.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                Won.targetGraphic.GetComponent<Image>().color = new Color(255 / 255f, 139 / 255f, 87 / 255f, 150 / 255f);
                ToBeWon.targetGraphic.GetComponent<Image>().color = new Color(255 / 255f, 139 / 255f, 87 / 255f, 255 / 255f);
                mCanClickWon = true;
                mCanClickToBeWon = false;
                await GetAllAchivements();
                FillAchievements(mAllAchievements);
            }
        }

        public async Task<bool> GetAllAchivements()
        {
            string url = Endpoint.classesURL + "/gamifications/achievements";
            var seembaResponse = await SeembaWebRequest.Get.HttpsGetJSON<AchievemenstsList>(url);
            mAllAchievements = seembaResponse.achievements;
            return true;
        }

        private void FillAchievements(AchievementItem[] items, bool doneOnly = false)
        {
            ClearContent();
            foreach (AchievementItem item in items)
            {
                if(item.done == doneOnly)
                {
                    GameObject AchievementItem = Instantiate(AchievementPrefab, ItemsContainer);
                    AchievementItem.GetComponent<RectTransform>().localScale = Vector3.one;
                    AchievementItem.GetComponent<AchievementsItemController>().Init(item);
                }
            }
        }

        public async Task<bool> GetMyAchivements()
        {
            string url = Endpoint.classesURL + "/gamifications/achievements";
            var seembaResponse = await SeembaWebRequest.Get.HttpsGetJSON<AchievemenstsList>(url);
            Debug.LogWarning(url);
            mMyAchievements = seembaResponse.achievements;
            return true;
        }

        void ClearContent()
        {
            foreach (Transform transform in ItemsContainer)
            {
                Destroy(transform.gameObject);
            }
        }
        #endregion
    }
}

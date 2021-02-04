using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class AchievementsController : MonoBehaviour
    {

        #region Script Parameters
        public Toggle               Won;
        public Toggle               ToBeWon;
        #endregion

        #region Fields
        private bool mCanClickWon = true;
        private bool mCanClickToBeWon = false;
        #endregion

        #region Unity Methods

        private void Start()
        {
            OnClickWon(!Won.isOn);
            OnClickToBeWon(!ToBeWon.isOn);
        }

        private void OnEnable()
        {
            ReershAchievements();
        }
        #endregion

        #region Methods
        public void OnClickWon(bool selected)
        {
            if (selected && mCanClickWon) 
            {
                Won.GetComponent<RectTransform>().localScale = Vector3.one;
                ToBeWon.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                mCanClickToBeWon = true;
                mCanClickWon = false;
            }
        }
        public void OnClickToBeWon(bool selected)
        {
            if (selected && mCanClickToBeWon)
            {
                ToBeWon.GetComponent<RectTransform>().localScale = Vector3.one;
                Won.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                mCanClickWon = true;
                mCanClickToBeWon = false;
            }
        }

        public void ReershAchievements()
        {
            Debug.LogWarning("refresh achievements");
        }
        #endregion
    }
}

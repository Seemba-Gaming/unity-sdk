using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class AchievementsController : MonoBehaviour
    {
        public Toggle               Won;
        public Toggle               ToBeWon;

        private bool mCanClickWon = true;
        private bool mCanClickToBeWon = false;
        public void OnClickWon(bool selected)
        {
            if(selected && mCanClickWon) 
            {
                Won.GetComponent<RectTransform>().localScale += new Vector3(0.1f, 0.1f, 0.1f);
                ToBeWon.GetComponent<RectTransform>().localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                Debug.LogWarning("A7maar");
                mCanClickToBeWon = true;
                mCanClickWon = false;
            }
        }
        public void OnClickToBeWon(bool selected)
        {
            if (selected && mCanClickToBeWon)
            {
                ToBeWon.GetComponent<RectTransform>().localScale += new Vector3(0.1f, 0.1f, 0.1f);
                Won.GetComponent<RectTransform>().localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                Debug.LogWarning("Asfaaar");
                mCanClickWon = true;
                mCanClickToBeWon = false;
            }
        }
    }
}

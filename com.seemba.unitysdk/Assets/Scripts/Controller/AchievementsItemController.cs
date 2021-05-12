using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class AchievementsItemController : MonoBehaviour
    {
        #region Script Parameters
        public Text Progress;
        public Image ProgressBar;
        public Text Title;
        public Text description;
        public Image AchievementIcon;
        public bool Done;
        #endregion

        #region Methods
        public void Init(AchievementItem item)
        {
            TranslationManager._instance.scene = "Achievements";
            Title.text = TranslationManager._instance.Get(item.name);
            description.text = TranslationManager._instance.Get(item.name + "_description");
            Progress.text = item.current_amount + "/" + item.total_amount;
            ProgressBar.fillAmount = (float)item.current_amount / item.total_amount;
        }
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class AchievementsItemController : MonoBehaviour
    {
        #region Script Parameters
        public string AchivementId;
        public string amount;
        public Text Title;
        public Text description;
        public Image AchievementIcon;
        #endregion

        #region Methods
        public void Init(AchievementItem item)
        {
            Title.text = item.name;
            description.text = item.description;
        }
        #endregion
    }
}

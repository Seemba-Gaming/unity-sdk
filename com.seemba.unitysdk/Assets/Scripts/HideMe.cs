using UnityEngine;

namespace SeembaSDK
{
    public class HideMe : MonoBehaviour
    {
        public GameObject[] HiddenElements;
        void Start()
        {
            if (Seemba.Get.IsSeemba)
            {
                for (int i = 0; i < HiddenElements.Length; i++)
                {
                    HiddenElements[i].SetActive(false);
                }
            }
        }
    }
}

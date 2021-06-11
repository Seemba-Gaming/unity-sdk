using UnityEngine;

namespace SeembaSDK
{
    public class LoaderController : MonoBehaviour
    {
        public Loader Loader;

        private GameObject mContent;
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            mContent = transform.GetChild(0).gameObject;
        }
        public void ShowLoader(string message = null, string close =null)
        {
            mContent.SetActive(true);
            Loader.title.text = message;
            if(close != null)
            {
                Loader.close.text = close;
            }
        }
        public void HideLoader()
        {
            mContent.SetActive(false);
        }
    }
}

using System;
using UnityEngine;

[CLSCompliant(false)]
public class LoaderController : MonoBehaviour
{
    public Loader Loader;

    private GameObject mContent;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        mContent = transform.GetChild(0).gameObject;
    }
    public void ShowLoader(string message = null)
    {
        mContent.SetActive(true);
        Loader.title.text = message;
    }
    public void HideLoader()
    {
        mContent.SetActive(false);
    }
}

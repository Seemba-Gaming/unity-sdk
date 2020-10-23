using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    #region Static
    public static PopupManager Get { get { return sInstance; } }
    private static PopupManager sInstance;
    #endregion

    #region Script Parameters
    public GameObject PopupPrefab;
    public PopupsController PopupController;
    public PopupsViewPresenter PopupViewPresenter;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        sInstance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        var popup = Instantiate(PopupPrefab) as GameObject;
        PopupController = popup.GetComponent<PopupsController>();
        PopupViewPresenter = popup.GetComponent<PopupsViewPresenter>();
    }
    #endregion

    #region Methods
    #endregion

    #region Implementation
    #endregion
}

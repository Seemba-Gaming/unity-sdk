using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupsController : MonoBehaviour
{

    public enum PopupType 
    {
        DUELS,
        INSUFFICIENT_BALANCE,
        INSUFFICIENT_BUBBLES,
        DEV_MODE,
        VPN,
        PROHIBITED_LOCATION,
        DOWNLOAD_FROM_STORE,
    }
   

    public static PopupType CURRENT_POPUP;
    private static PopupsController _Instance;

    private void OnEnable()
    {
        _Instance = this;
    }
    public static PopupsController getInstance()
    {
        if (!_Instance)
            _Instance = new PopupsController();
        return _Instance;
    }

    public async void ShowPopup(PopupType popupType, object[] _params)
    {
       
        //Load Popups Scene
        LoadPopup();
        //Set Current popup type
        CURRENT_POPUP = popupType;
        //Init Params
        PopupsViewPresenter._params = _params;
        PopupsViewPresenter.popup = popupType;

    }
    public void LoadPopup()
    {
        SceneManager.LoadSceneAsync("Popup", LoadSceneMode.Additive);
    }



}

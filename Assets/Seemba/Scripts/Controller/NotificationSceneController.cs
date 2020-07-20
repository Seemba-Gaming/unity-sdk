using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NotificationSceneController : MonoBehaviour
{
    private static NotificationSceneController _instance;
    public static NotificationSceneController instance;
    bool isOnAndroid = false;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        instance = _instance;
        DontDestroyOnLoad(this);
#if UNITY_ANDROID
        isOnAndroid = true;
        Screen.fullScreen = false;
#endif
    }
    void OnApplicationPause(bool appPaused)
    {
        if (!isOnAndroid || Application.isEditor) { return; }
        if (!appPaused)
        {
            //Returning to Application
            Debug.Log("Application Resumed");
            StartCoroutine(LoadSceneFromFCM());
        }
        else
        {
            //Leaving Application
            Debug.Log("Application Paused");
        }
    }
    IEnumerator LoadSceneFromFCM()
    {
    #if UNITY_ANDROID
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject curActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject curIntent = curActivity.Call<AndroidJavaObject>("getIntent");
        string sceneToLoad = curIntent.Call<string>("getStringExtra", "sceneToOpen");
        ChallengeManager.CurrentChallengeId = curIntent.Call<string>("getStringExtra", "challengeId");
        Scene curScene = SceneManager.GetActiveScene();
        if (!string.IsNullOrEmpty(sceneToLoad) && sceneToLoad != curScene.name)
        {
            // If the current scene is different than the intended scene to load,
            // load the intended scene. This is to avoid reloading an already acive
            // scene.
            Debug.Log("Loading Scene: " + sceneToLoad);
            Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
            Handheld.StartActivityIndicator();
            yield return new WaitForSeconds(0f);
            SceneManager.LoadScene(sceneToLoad);
        }
    #else
         yield return null;
    #endif
    }
}

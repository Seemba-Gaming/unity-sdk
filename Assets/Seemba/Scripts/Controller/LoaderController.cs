using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoaderController : MonoBehaviour
{
    private static AsyncOperation asyncLoad;
    // Use this for initialization
    public static void ShowLoader()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Loader");
    }
    public static void HideLoader()
    {
        //StartCoroutine(UnloadLoader());
    }
    static IEnumerator UnloadLoader()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        //This is particularly good for creating loading screens. You could also load the Scene by build //number.
        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        { }
        SceneManager.UnloadSceneAsync("Loader");
        yield return null;
    }
    void Start()
    {

    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameListItemController : MonoBehaviour
{
    ArrayList Items;
    GamesManager GamesManager;
    public GameObject ContentPanel;
    // Use this for initialization
    public Text title;
    public Image icon;
    public Button Download;
    void Start()
    {
        /* EventsController nbs =new EventsController();
		 //nbs.ShowPopup("Popup");
		Items = new ArrayList (); 
		GamesManager = new GamesManager ();
		GameObject.Find("PanelWaiting").transform.localScale=new Vector3(1,1,1);
		UnityThreading.ActionThread thread;
		Items=GamesManager.getPromotions(GamesManager.gameId);
		thread = UnityThreadHelper.CreateThread (() => {
			//GamesManager.gameId);
			UnityThreadHelper.Dispatcher.Dispatch (() => {
				if(Items!=null) {
				if(Items.Count!=0){
				System.Random random=new System.Random();
				int RandomInt=random.Next(1,Items.Count);
				int counter=0;
				foreach (Game item in Items) {
					counter++;
					if (counter == RandomInt) {
							string bundleId = item.bundle_id; // your target bundle id
							string AppStoreId = item.appstore_id;
							title.text=item.name.ToUpper();
							StartCoroutine(ShowIcon(item.icon));
			  	Download.onClick.AddListener (() => {
				#if UNITY_ANDROID && !UNITY_EDITOR
				bool fail = false;
				AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
				AndroidJavaObject launchIntent = null;
				try
				{
					launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage",bundleId);
				}
				catch (System.Exception e)
				{
					fail = true;
				}
				if (fail)
				{ //open app in store
					Application.OpenURL("https://play.google.com/store/apps/details?id="+bundleId+"&hl=fr");
				}
				else //open the app
					ca.Call("startActivity",launchIntent);
				up.Dispose();
				ca.Dispose();
				packageManager.Dispose();
				launchIntent.Dispose();
							#else
							#if UNITY_EDITOR 
							//Debug.Log (bundleId);
							Application.OpenURL ("https://play.google.com/store/apps/details?id=" + bundleId + "&hl=fr");
							#endif
							#endif
							#if UNITY_IOS
				string AppStore=newItem.transform.GetChild (4).gameObject.GetComponent<Text>().text;
				Application.OpenURL("https://itunes.apple.com/app/id"+AppStoreId);
							#endif
						});
					}
				}
				}else{
					SceneManager.UnloadScene("Popup");
				}
				}
				else{
					SceneManager.UnloadScene("Popup");
				}
			});
		}); */
    }
    public IEnumerator ShowIcon(string url)
    {
        var www = new WWW(url);
        yield return www;
        var texture = www.texture;
        icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        GameObject.Find("PanelWaiting").transform.localScale = new Vector3(0, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
    }
}

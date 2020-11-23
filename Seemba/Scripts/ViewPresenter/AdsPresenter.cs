using UnityEngine;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;
// Example script showing how to invoke the Google Mobile Ads Unity plugin.
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class AdsPresenter : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    /* private BannerView bannerView;
	private InterstitialAd interstitial;
	private RewardBasedVideoAd rewardBasedVideo;
	private float deltaTime = 0.0f;
	private static string outputMessage = string.Empty;
	public Button ExtraBubbles;
	public Image LoadingExtraBubbles;
	public ParticleSystem EffectExtraBubbles;
	public static string OutputMessage
	{
		set { outputMessage = value; }
	}
	public void OnEnable()
	{
		#if UNITY_ANDROID
		string appId = "ca-app-pub-6734996653225521~9183281102";
		#elif UNITY_IPHONE
		string appId = "ca-app-pub-6734996653225521~2123048735";
		#else
		string appId = "unexpected_platform";
		#endif
		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);
		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;
		// RewardBasedVideoAd is a singleton, so handlers should only be registered once.
		this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
		this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
		this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
		this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
		this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
		this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
		this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;
		ExtraBubbles.onClick.AddListener (() => {
		bool deviceIsIphoneX = false;
		#if UNITY_IOS
		deviceIsIphoneX= UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneUnknown;
		#endif
		if (deviceIsIphoneX) {
			EventsController nbs=new EventsController();
			nbs.ShowPopupError("UnknownDevice");
						// Do something for iPhone X
		 }else {
		LoadingExtraBubbles.gameObject.SetActive(true);
			this.RequestRewardBasedVideo ();
			this.RequestInterstitial ();
		}
		});
		}
		public void Update()
		{
		// Calculate simple moving average for time to render screen. 0.1 factor used as smoothing
		// value.
		this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
		}
		// Returns an ad request with custom ad targeting.
		private AdRequest CreateAdRequest()
		{
		return new AdRequest.Builder()
		.AddTestDevice(AdRequest.TestDeviceSimulator)
		.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
		.AddKeyword("game")
		.SetGender(Gender.Male)
		.SetBirthday(new DateTime(1985, 1, 1))
		.TagForChildDirectedTreatment(false)
		.AddExtra("color_bg", "9B30FF")
		.Build();
		}
		private void RequestBanner()
		{
		// These ad units are configured to always serve test ads.
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/6300978111";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/2934735716";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		// Clean up banner ad before creating a new one.
		if (this.bannerView != null)
		{
		this.bannerView.Destroy();
		}
		// Create a 320x50 banner at the top of the screen.
		this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
		// Register for ad events.
		this.bannerView.OnAdLoaded += this.HandleAdLoaded;
		this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
		this.bannerView.OnAdOpening += this.HandleAdOpened;
		this.bannerView.OnAdClosed += this.HandleAdClosed;
		this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;
		// Load a banner ad.
		this.bannerView.LoadAd(this.CreateAdRequest());
		}
		private void RequestInterstitial()
		{
		// These ad units are configured to always serve test ads.
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-6734996653225521/3530186090";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-6734996653225521/8072066335";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		// Clean up interstitial ad before creating a new one.
		if (this.interstitial != null)
		{
		this.interstitial.Destroy();
		}
		// Create an interstitial.
		this.interstitial = new InterstitialAd(adUnitId);
		// Register for ad events.
		this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
		this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
		this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
		this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
		this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;
		// Load an interstitial ad.
		this.interstitial.LoadAd(this.CreateAdRequest());
		}
		private void RequestNativeExpressAdView()
		{
		// These ad units are configured to always serve test ads.
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/1072772517";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/2562852117";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		}
		private void RequestRewardBasedVideo()
		{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-6734996653225521/6903467629";
		//string adUnitId = "ca-app-pub-3940256099942544/5224354917";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-6734996653225521/3995701863";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
		}
		private void ShowInterstitial()
		{
		if (this.interstitial.IsLoaded())
		{
		this.interstitial.Show();
		}
		else
		{
		MonoBehaviour.print("Interstitial is not ready yet");
		}
		}
		private void ShowRewardBasedVideo()
		{
		if (this.rewardBasedVideo.IsLoaded())
		{
		this.rewardBasedVideo.Show();
		}
		else
		{
		MonoBehaviour.print("Reward based video ad is not ready yet");
		}
		}
		#region Banner callback handlers
		public void HandleAdLoaded(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdLoaded event received");
		}
		public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{ 
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
		}
		public void HandleAdOpened(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdOpened event received");
		}
		public void HandleAdClosed(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdClosed event received");
		}
		public void HandleAdLeftApplication(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdLeftApplication event received");
		}
		#endregion
		#region Interstitial callback handlers
		public void HandleInterstitialLoaded(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleInterstitialLoaded event received");
		this.ShowInterstitial ();
		}
		public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
		MonoBehaviour.print(
		"HandleInterstitialFailedToLoad event received with message: " + args.Message);
		LoadingExtraBubbles.gameObject.SetActive(false);
		}
		public void HandleInterstitialOpened(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleInterstitialOpened event received");
		}
	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
			MonoBehaviour.print("HandleInterstitialClosed event received");
		//Update User and show animation
		this.interstitial.Destroy ();
			winRewardFromAds();
	}
		public void HandleInterstitialLeftApplication(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleInterstitialLeftApplication event received");
		}
		#endregion
		#region Native express ad callback handlers
		public void HandleNativeExpressAdLoaded(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleNativeExpressAdAdLoaded event received");
		}
		public void HandleNativeExpresseAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
		MonoBehaviour.print(
		"HandleNativeExpressAdFailedToReceiveAd event received with message: " + args.Message);
		}
		public void HandleNativeExpressAdOpened(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleNativeExpressAdAdOpened event received");
		}
		public void HandleNativeExpressAdClosed(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleNativeExpressAdAdClosed event received");
		}
		public void HandleNativeExpressAdLeftApplication(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleNativeExpressAdAdLeftApplication event received");
		}
		#endregion
		#region RewardBasedVideo callback handlers
		public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		this.ShowRewardBasedVideo ();
		}
		public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
		MonoBehaviour.print(
		"HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
		}
		public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
		}
		public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
		}
		public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
		{
		}
		public void HandleRewardBasedVideoRewarded(object sender, Reward args)
		{
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print(
		"HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		//Update User and show animation
		winRewardFromAds();
		}
		void winRewardFromAds(){
			LoadingExtraBubbles.gameObject.SetActive(false);
			UserManager um = new UserManager ();
			string UserId = um.getCurrentUserId ();
			string UserSessionToken = um.getCurrentSessionToken ();
			string[] attrib = { "bubble_credit"};
			string[] value = { ((int.Parse (UserManager.CurrentWater.ToString ()) + 5)).ToString ()};
		//UnityThreadHelper.CreateThread (() => {
			um.UpdateUserByField (UserId, UserSessionToken, attrib, value);
			um.bubblesTrack (UserId, UserSessionToken, GamesManager.gameId, GamesManager.ADS_WATCHED);
			UnityThreadHelper.Dispatcher.Dispatch (() => {
				UserManager.CurrentWater = ((int.Parse (UserManager.CurrentWater.ToString ()) + 5)).ToString ();
				Text PlayerWater = GameObject.Find ("virtual_money").GetComponent<Text> ();
				PlayerWater.text = UserManager.CurrentWater;
				EffectExtraBubbles.Play();
			});
		//});
		}
		public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
		}
		#endregion
		*/
}

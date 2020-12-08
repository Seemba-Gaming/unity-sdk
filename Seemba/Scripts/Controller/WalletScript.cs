using UnityEngine;
using System;
using System.Threading;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class WalletScript : MonoBehaviour
	{
		#region Script Parameters
		public static float LastCredit;
		public static bool showPopupMail;
		public static string moneyToAdd;
		public InputField ChargeAmount;
		public WalletTogglePresenter Add5Euro;
		public WalletTogglePresenter Add10Euro;
		public WalletTogglePresenter Add15Euro;
		public WalletTogglePresenter Add20Euro;
		public WalletTogglePresenter AddOtherAmount;
		public Text EuroLabel;
		#endregion

		#region Unity Methods
		private void Start()
		{

		}
		#endregion

		#region Methods
		public async void creditClick()
		{
			if (string.IsNullOrEmpty(UserManager.Get.CurrentUser.country_code))
			{
				UserManager.Get.CurrentUser.country_code = await UserManager.Get.GetGeoLoc();
			}

			if (CountryController.checkCountry(UserManager.Get.CurrentUser.country_code))
			{
				StartCoroutine(EventsController.Get.checkInternetConnection(async (isConnected) =>
				{

					LoaderManager.Get.LoaderController.HideLoader();
					if (isConnected == true)
					{
						if (Add5Euro.IsSelected)
						{
							await Add5Euro.AddAmountAsync();
						}
						else if (Add10Euro.IsSelected)
						{
							await Add10Euro.AddAmountAsync();
						}
						else if (Add15Euro.IsSelected)
						{
							await Add15Euro.AddAmountAsync();
						}
						else if (Add20Euro.IsSelected)
						{
							await Add20Euro.AddAmountAsync();
						}
						else
						{
							await AddOtherAmount.AddOtherAmountAsync();
						}
					}
					else
					{
						ConnectivityController.CURRENT_ACTION = ConnectivityController.CREDIT_ACTION;
						PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
					}
				}));
			}
			else
			{
				UnityThreading.ActionThread thread;
				thread = UnityThreadHelper.CreateThread(() =>
				{
					Thread.Sleep(300);
					UnityThreadHelper.Dispatcher.Dispatch(() =>
					{
						PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_PROHIBITED_LOCATION_WALLET, PopupsText.Get.ProhibitedLocationWallet());
					});
				});
			}
		}
		public void OnEnable()
		{
			if (ChargeAmount != null)
			{
				ChargeAmount.onValueChanged.AddListener(delegate
				{
					if (string.IsNullOrEmpty(ChargeAmount.text))
					{
						EuroLabel.text = "";
					}
					else
					{
						EuroLabel.text = CurrencyManager.CURRENT_CURRENCY;
					}
				});
			}
		}
		#endregion

		#region Implementations
		private bool ValidMail(string mail_address)
		{
			Regex myRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
			return myRegex.IsMatch(mail_address);
		}
		#endregion
	}
}
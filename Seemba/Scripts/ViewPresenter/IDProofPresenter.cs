using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class IDProofPresenter : MonoBehaviour
    {

        #region Script Parameters
        public GameObject IDAddressSuccess;
        public GameObject IDFrontSuccess;
        public GameObject IDBackSuccess;
        public GameObject IDPassportSuccess;

        public GameObject IDFrontWaiting;
        public GameObject IDBackWaiting;
        public GameObject IDPassportWaiting;

        public Button IDFront;
        public Button IDBack;
        public Button IDPassport;

        public GameObject IDFrontDisabled;
        public GameObject IDBackDisabled;
        public GameObject IDPassportDisabled;
        #endregion
        #region Fields
        private WithdrawManager wm = new WithdrawManager();
        private string userToken;
        #endregion

        #region Unity Methods
        async void OnEnable()
        {
            userToken = UserManager.Get.getCurrentSessionToken();
            LoaderManager.Get.LoaderController.ShowLoader(null);

            User user = await UserManager.Get.getUser();
            var account = await wm.accountVerificationJSON(userToken);

            LoaderManager.Get.LoaderController.HideLoader();

            if (string.IsNullOrEmpty(user.birthdate) || string.IsNullOrEmpty(user.address) || string.IsNullOrEmpty(user.firstname) || string.IsNullOrEmpty(user.lastname) || string.IsNullOrEmpty(user.zipcode) || string.IsNullOrEmpty(user.city) || string.IsNullOrEmpty(user.phone))
            {
                showMissingInfoPopup();
            }
            else
            {
                string documentFrontID = null;
                string documentBackID = null;
                string documentAddress = null;
                var accountStatus = account["verification_status"].Value;
                Debug.Log(accountStatus);
                try
                {
                    documentFrontID = account["account"]["individual"]["verification"]["document"]["front"].Value;
                }
                catch (NullReferenceException) { }
                try
                {
                    documentBackID = account["account"]["individual"]["verification"]["document"]["back"].Value;
                }
                catch (NullReferenceException) { }
                try
                {
                    documentAddress = account["account"]["individual"]["verification"]["additional_document"]["front"].Value;
                }
                catch (NullReferenceException) { }

                if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED))
                {
                    IDAddressSuccess.SetActive(true);
                    if (!string.IsNullOrEmpty(documentBackID))
                    {
                        IDFrontSuccess.SetActive(true);
                        IDBackSuccess.SetActive(true);
                        IDPassport.interactable = false;
                        IDPassportDisabled.SetActive(true);
                    }
                    else
                    {
                        IDPassportSuccess.SetActive(true);
                        IDFront.interactable = false;
                        IDBack.interactable = false;
                        IDFrontDisabled.SetActive(true);
                        IDBackDisabled.SetActive(true);
                    }
                }
                if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING))
                {
                    IDAddressSuccess.SetActive(true);
                    if (!string.IsNullOrEmpty(documentBackID))
                    {
                        IDFrontWaiting.SetActive(true);
                        IDBackWaiting.SetActive(true);

                        IDPassport.interactable = false;
                        IDPassportDisabled.SetActive(true);
                    }
                    else
                    {
                        IDPassportWaiting.SetActive(true);
                        IDFront.interactable = false;
                        IDBack.interactable = false;

                        IDFrontDisabled.SetActive(true);
                        IDBackDisabled.SetActive(true);
                    }
                }
                if (!string.IsNullOrEmpty(documentBackID) && user.id_proof_2_uploaded)
                {
                    IDBackWaiting.SetActive(true);
                    IDPassport.interactable = false;

                    IDPassportDisabled.SetActive(true);

                }
                if (!string.IsNullOrEmpty(documentFrontID) && user.id_proof_1_uploaded)
                {
                    IDFrontWaiting.SetActive(true);
                    IDPassport.interactable = false;

                    IDPassportDisabled.SetActive(true);

                }
                if (!string.IsNullOrEmpty(documentBackID) && user.passport_uploaded)
                {
                    IDPassportWaiting.SetActive(true);
                    IDFront.interactable = false;
                    IDBack.interactable = false;

                    IDFrontDisabled.SetActive(true);
                    IDBackDisabled.SetActive(true);
                }
            }

        }
        #endregion

        #region Methods
        public void missingInfoContinue()
        {
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(300);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    PopupManager.Get.PopupViewPresenter.HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.PersonalInfo.gameObject, true);
                });
            });
        }
        #endregion

        #region Implementation
        void showMissingInfoPopup()
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_MISSING_INFO, PopupsText.Get.MissingInfo(), "Idproof");
        }
        #endregion
    }
}

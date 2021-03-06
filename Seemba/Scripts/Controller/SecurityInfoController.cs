﻿using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class SecurityInfoController : MonoBehaviour
    {
        #region Script Parameters
        public Text email;
        public Button ChangePassword;
        #endregion

        #region Fields
        private InputField CurrentPassword;
        private InputField NewPassword;
        private InputField ConfirmPassword;
        private Button UpdatePassword;
        private Button nextCurrentPassword;
        private Button greatPasswordChanged;
        private User CurrentUser;
        #endregion

        #region Unity Methods
        private void Start()
        {
            CurrentUser = UserManager.Get.CurrentUser;
            email.text = CurrentUser.email.Substring(0, 4);
            for (int i = 0; i < CurrentUser.email.Length; i++)
            {
                email.text = email.text + "*";
            }

            CurrentPassword = PopupManager.Get.PopupViewPresenter.PopupCurrentPassInputField;
            NewPassword = PopupManager.Get.PopupViewPresenter.PopupUpdatePassNewPasswordInputField;
            nextCurrentPassword = PopupManager.Get.PopupViewPresenter.PopupCurrentPassconfirmButton;
            ConfirmPassword = PopupManager.Get.PopupViewPresenter.PopupUpdatePassConfirmPasswordInputField;
            UpdatePassword = PopupManager.Get.PopupViewPresenter.PopupUpdatePassconfirmButton;
            greatPasswordChanged = PopupManager.Get.PopupViewPresenter.PopupInfoConfirmButton;

            CurrentPassword.onValueChanged.AddListener(async delegate
            {
                nextCurrentPassword.interactable = false;
                await CurrentPasswordListener(); 
            });

            ConfirmPassword.onValueChanged.AddListener(delegate
            {
                if (NewPassword.text == ConfirmPassword.text)
                {
                    UpdatePassword.interactable = true;
                }
                else
                {
                    UpdatePassword.interactable = false;
                }
            });

            nextCurrentPassword.onClick.AddListener(() =>
            {
                PopupManager.Get.PopupViewPresenter.HidePopupContent(PopupManager.Get.PopupController.PopupCurrentPassword);
                PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_UPDATE_PASSWORD, PopupsText.Get.UpdatePassword());
                CurrentPassword.text = "";
                nextCurrentPassword.interactable = false;
            });

            UpdatePassword.onClick.AddListener(delegate
            {
                OnClickUpdaetPassword();
                PopupManager.Get.PopupViewPresenter.ClearPopups();
            });
        }

        public async void OnClickUpdaetPassword()
        {
            Debug.LogWarning("here");
            LoaderManager.Get.LoaderController.ShowLoader();
            PopupManager.Get.PopupViewPresenter.HidePopupContent(PopupManager.Get.PopupController.PopupUpdatePassword);
            var res = await UserManager.Get.updatePassword(UserManager.Get.CurrentUser.email, NewPassword.text);
            LoaderManager.Get.LoaderController.HideLoader();
            if (res)
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_PASSWORD_UPDATED, PopupsText.Get.PasswordUpdated());
            }
            else
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
            }
        }
        #endregion

        #region Methods
        public void OnClickChangePassword()
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_CURRENT_PASSWORD, PopupsText.Get.CurrentPassword());
        }

        public async Task<bool> CurrentPasswordListener()
        {
            string res = await UserManager.Get.logingIn(CurrentUser.email, CurrentPassword.text);
            if (res == "success")
            {
                nextCurrentPassword.interactable = true;
            }
            else
            {
                nextCurrentPassword.interactable = false;
            }
            return true;
        }
        #endregion
    }
}

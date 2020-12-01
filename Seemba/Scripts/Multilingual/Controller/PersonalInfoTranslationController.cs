using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
#pragma warning disable 0649
    public class PersonalInfoTranslationController : MonoBehaviour
    {
        [Header("----------------------PERSONAL INFO----------------------")]
        [SerializeField]
        private Text last_name;
        [SerializeField]
        private Text BackButton;
        [SerializeField]
        private Text first_name, date_of_birth, address, city, zip, country, phone;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager.scene = "Home";
            BackButton.text = TranslationManager.Get("back_button");
            TranslationManager.scene = "PersonalInfo";
            first_name.text = TranslationManager.Get("first_name") != string.Empty ? TranslationManager.Get("first_name") : first_name.text;
            last_name.text = TranslationManager.Get("last_name") != string.Empty ? TranslationManager.Get("last_name") : last_name.text;
            date_of_birth.text = TranslationManager.Get("date_of_birth") != string.Empty ? TranslationManager.Get("date_of_birth") : date_of_birth.text;
            address.text = TranslationManager.Get("address") != string.Empty ? TranslationManager.Get("address") : address.text;
            city.text = TranslationManager.Get("city") != string.Empty ? TranslationManager.Get("city") : city.text;
            zip.text = TranslationManager.Get("zip") != string.Empty ? TranslationManager.Get("zip") : zip.text;
            country.text = TranslationManager.Get("country") != string.Empty ? TranslationManager.Get("country") : country.text;
            phone.text = TranslationManager.Get("phone") != string.Empty ? TranslationManager.Get("phone") : phone.text;
        }
    }
}
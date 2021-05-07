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
            TranslationManager._instance.scene = "Home";
            BackButton.text = TranslationManager._instance.Get("back_button");
            TranslationManager._instance.scene = "PersonalInfo";
            first_name.text = TranslationManager._instance.Get("first_name") != string.Empty ? TranslationManager._instance.Get("first_name") : first_name.text;
            last_name.text = TranslationManager._instance.Get("last_name") != string.Empty ? TranslationManager._instance.Get("last_name") : last_name.text;
            date_of_birth.text = TranslationManager._instance.Get("date_of_birth") != string.Empty ? TranslationManager._instance.Get("date_of_birth") : date_of_birth.text;
            address.text = TranslationManager._instance.Get("address") != string.Empty ? TranslationManager._instance.Get("address") : address.text;
            city.text = TranslationManager._instance.Get("city") != string.Empty ? TranslationManager._instance.Get("city") : city.text;
            zip.text = TranslationManager._instance.Get("zip") != string.Empty ? TranslationManager._instance.Get("zip") : zip.text;
            country.text = TranslationManager._instance.Get("country") != string.Empty ? TranslationManager._instance.Get("country") : country.text;
            phone.text = TranslationManager._instance.Get("phone") != string.Empty ? TranslationManager._instance.Get("phone") : phone.text;
        }
    }
}
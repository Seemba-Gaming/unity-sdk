using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    #pragma warning disable 649
    public class BankingTranslationController : MonoBehaviour
    {
        [SerializeField]
        private Text cardholder_full_name,
            wrong_cardholder_name,
            card_number,
            wrong_card_number,
            expiry_date,
            wrong_expiry_date,
            security_code,
            wrong_security_code,
            i_agree_to_Seemba,
            and,
            terms_conditions,
            privacy_policy,
            credit,
            secured_payment,
            transaction_fees_may_apply,
            back_button;
        void Start()
        {
            TranslationManager._instance.scene = "BankingInfo";
            cardholder_full_name.text = TranslationManager._instance.Get("cardholder_full_name") != string.Empty ? TranslationManager._instance.Get("cardholder_full_name") : cardholder_full_name.text;
            wrong_cardholder_name.text = TranslationManager._instance.Get("wrong_cardholder_name") != string.Empty ? TranslationManager._instance.Get("wrong_cardholder_name") : wrong_cardholder_name.text;
            card_number.text = TranslationManager._instance.Get("card_number") != string.Empty ? TranslationManager._instance.Get("card_number") : card_number.text;
            wrong_card_number.text = TranslationManager._instance.Get("wrong_card_number") != string.Empty ? TranslationManager._instance.Get("wrong_card_number") : wrong_card_number.text;
            expiry_date.text = TranslationManager._instance.Get("expiry_date") != string.Empty ? TranslationManager._instance.Get("expiry_date") : expiry_date.text;
            wrong_expiry_date.text = TranslationManager._instance.Get("wrong_expiry_date") != string.Empty ? TranslationManager._instance.Get("wrong_expiry_date") : wrong_expiry_date.text;
            security_code.text = TranslationManager._instance.Get("security_code") != string.Empty ? TranslationManager._instance.Get("security_code") : security_code.text;
            wrong_security_code.text = TranslationManager._instance.Get("wrong_security_code") != string.Empty ? TranslationManager._instance.Get("wrong_security_code") : wrong_security_code.text;
            i_agree_to_Seemba.text = TranslationManager._instance.Get("i_agree_to_Seemba") != string.Empty ? TranslationManager._instance.Get("i_agree_to_Seemba") : i_agree_to_Seemba.text;
            and.text = TranslationManager._instance.Get("and") != string.Empty ? TranslationManager._instance.Get("and") : and.text;
            terms_conditions.text = TranslationManager._instance.Get("terms_conditions") != string.Empty ? TranslationManager._instance.Get("terms_conditions") : terms_conditions.text;
            privacy_policy.text = TranslationManager._instance.Get("privacy_policy") != string.Empty ? TranslationManager._instance.Get("privacy_policy") : privacy_policy.text;
            credit.text = TranslationManager._instance.Get("credit") != string.Empty ? TranslationManager._instance.Get("credit") : credit.text;
            secured_payment.text = TranslationManager._instance.Get("secured_payment") != string.Empty ? TranslationManager._instance.Get("secured_payment") : secured_payment.text;
            transaction_fees_may_apply.text = TranslationManager._instance.Get("transaction_fees_may_apply");
            TranslationManager._instance.scene = "Home";
            back_button.text = TranslationManager._instance.Get("back_button");

        }
    }
}
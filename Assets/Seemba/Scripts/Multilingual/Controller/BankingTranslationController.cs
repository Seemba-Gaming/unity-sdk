using UnityEngine;
using UnityEngine.UI;
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
        secured_payment;
    void Start()
    {
        TranslationManager.scene = "BankingInfo";
        cardholder_full_name.text = TranslationManager.Get("cardholder_full_name") != string.Empty ? TranslationManager.Get("cardholder_full_name") : cardholder_full_name.text;
        wrong_cardholder_name.text = TranslationManager.Get("wrong_cardholder_name") != string.Empty ? TranslationManager.Get("wrong_cardholder_name") : wrong_cardholder_name.text;
        card_number.text = TranslationManager.Get("card_number") != string.Empty ? TranslationManager.Get("card_number") : card_number.text;
        wrong_card_number.text = TranslationManager.Get("wrong_card_number") != string.Empty ? TranslationManager.Get("wrong_card_number") : wrong_card_number.text;
        expiry_date.text = TranslationManager.Get("expiry_date") != string.Empty ? TranslationManager.Get("expiry_date") : expiry_date.text;
        wrong_expiry_date.text = TranslationManager.Get("wrong_expiry_date") != string.Empty ? TranslationManager.Get("wrong_expiry_date") : wrong_expiry_date.text;
        security_code.text = TranslationManager.Get("security_code") != string.Empty ? TranslationManager.Get("security_code") : security_code.text;
        wrong_security_code.text = TranslationManager.Get("wrong_security_code") != string.Empty ? TranslationManager.Get("wrong_security_code") : wrong_security_code.text;
        i_agree_to_Seemba.text = TranslationManager.Get("i_agree_to_Seemba") != string.Empty ? TranslationManager.Get("i_agree_to_Seemba") : i_agree_to_Seemba.text;
        and.text = TranslationManager.Get("and") != string.Empty ? TranslationManager.Get("and") : and.text;
        terms_conditions.text = TranslationManager.Get("terms_conditions") != string.Empty ? TranslationManager.Get("terms_conditions") : terms_conditions.text;
        privacy_policy.text = TranslationManager.Get("privacy_policy") != string.Empty ? TranslationManager.Get("privacy_policy") : privacy_policy.text;
        credit.text = TranslationManager.Get("credit") != string.Empty ? TranslationManager.Get("credit") : credit.text;
        secured_payment.text = TranslationManager.Get("secured_payment") != string.Empty ? TranslationManager.Get("secured_payment") : secured_payment.text;
    }
}

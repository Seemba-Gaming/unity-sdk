using UnityEngine;
using UnityEngine.UI;
public class WithdrawalInfoTranslationController : MonoBehaviour
{
    //IDENTITY VIEW
    [Header("----------------------HEADER----------------------")]
    [SerializeField]
    private Text financial_info;
    [SerializeField]
    private Text personal_info;
    [SerializeField]
    private Text id_proof;
    [SerializeField]
    private Text next_iban, next_personal_info, withdraw;
    [Header("IDENTITY")]
    [Header("----------------------ID PROOF----------------------")]
    [SerializeField]
    private Text identity_front;
    [SerializeField]
    private Text
        identity_back,
        waiting_identity_text_front, waiting_identity_text_back,
        verified_identity_front, verified_identity_back;
    //PASSPORT VIEW
    [Header("PASSPORT")]
    [SerializeField]
    private Text passport_proof;
    [SerializeField]
    private Text
                 waiting_passport_text_front,
                 verified_passport_front;
    //RESIDENCY VIEW
    [Header("RESIDENCY")]
    [SerializeField]
    private Text address_proof;
    [SerializeField]
    private Text
                 waiting_residency_text_front,
                 verified_residency_front;
    [Header("----------------------FINANCIAL INFO----------------------")]
    [SerializeField]
    private Text enter_your_iban;
    [SerializeField]
    private Text enter_your_bic;
    [Header("----------------------PERSONAL INFO----------------------")]
    [SerializeField]
    private Text last_name;
    [SerializeField]
    private Text first_name, date_of_birth, address, city, zip, country, phone;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "PersonalInfo";
        first_name.text = TranslationManager.Get("first_name") != string.Empty ? TranslationManager.Get("first_name") : first_name.text;
        last_name.text = TranslationManager.Get("last_name") != string.Empty ? TranslationManager.Get("last_name") : last_name.text;
        date_of_birth.text = TranslationManager.Get("date_of_birth") != string.Empty ? TranslationManager.Get("date_of_birth") : date_of_birth.text;
        address.text = TranslationManager.Get("address") != string.Empty ? TranslationManager.Get("address") : address.text;
        city.text = TranslationManager.Get("city") != string.Empty ? TranslationManager.Get("city") : city.text;
        zip.text = TranslationManager.Get("zip") != string.Empty ? TranslationManager.Get("zip") : zip.text;
        country.text = TranslationManager.Get("country") != string.Empty ? TranslationManager.Get("country") : country.text;
        phone.text = TranslationManager.Get("phone") != string.Empty ? TranslationManager.Get("phone") : phone.text;
        TranslationManager.scene = "WithdrawalInfo";
        personal_info.text = TranslationManager.Get("personal_info") != string.Empty ? TranslationManager.Get("personal_info") : personal_info.text;
        financial_info.text = TranslationManager.Get("financial_info") != string.Empty ? TranslationManager.Get("financial_info") : financial_info.text;
        id_proof.text = TranslationManager.Get("id_proof") != string.Empty ? TranslationManager.Get("id_proof") : id_proof.text;
        next_iban.text = TranslationManager.Get("next") != string.Empty ? TranslationManager.Get("next") : next_iban.text;
        next_personal_info.text = TranslationManager.Get("next") != string.Empty ? TranslationManager.Get("next") : next_personal_info.text;
        withdraw.text = TranslationManager.Get("withdraw") != string.Empty ? TranslationManager.Get("withdraw") : withdraw.text;
        enter_your_iban.text = TranslationManager.Get("enter_your_iban") != string.Empty ? TranslationManager.Get("enter_your_iban") : enter_your_iban.text;
        enter_your_bic.text = TranslationManager.Get("enter_your_bic") != string.Empty ? TranslationManager.Get("enter_your_bic") : enter_your_bic.text;
        TranslationManager.scene = "IDProof";
        //ID VIEW
        identity_front.text = TranslationManager.Get("identity_front") != string.Empty ? TranslationManager.Get("identity_front") : identity_front.text;
        identity_back.text = TranslationManager.Get("identity_back") != string.Empty ? TranslationManager.Get("identity_back") : identity_back.text;
        waiting_identity_text_front.text = TranslationManager.Get("waiting_text") != string.Empty ? TranslationManager.Get("waiting_text") : waiting_identity_text_front.text;
        waiting_identity_text_back.text = TranslationManager.Get("waiting_text") != string.Empty ? TranslationManager.Get("waiting_text") : waiting_identity_text_back.text;
        verified_identity_front.text = TranslationManager.Get("verified") != string.Empty ? TranslationManager.Get("verified") : verified_identity_front.text;
        verified_identity_back.text = TranslationManager.Get("verified") != string.Empty ? TranslationManager.Get("verified") : verified_identity_back.text;
        //PASSPORT VIEW
        passport_proof.text = TranslationManager.Get("passport_proof") != string.Empty ? TranslationManager.Get("passport_proof") : passport_proof.text;
        waiting_passport_text_front.text = TranslationManager.Get("waiting_text") != string.Empty ? TranslationManager.Get("waiting_text") : waiting_passport_text_front.text;
        verified_passport_front.text = TranslationManager.Get("verified") != string.Empty ? TranslationManager.Get("verified") : identity_back.text;
        //RESIDENCY VIEW
        address_proof.text = TranslationManager.Get("address_proof") != string.Empty ? TranslationManager.Get("address_proof") : address_proof.text;
        waiting_residency_text_front.text = TranslationManager.Get("waiting_text") != string.Empty ? TranslationManager.Get("waiting_text") : waiting_passport_text_front.text;
        verified_residency_front.text = TranslationManager.Get("verified") != string.Empty ? TranslationManager.Get("verified") : identity_back.text;
    }
}

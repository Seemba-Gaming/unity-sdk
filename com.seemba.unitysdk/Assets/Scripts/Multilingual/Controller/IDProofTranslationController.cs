using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class IDProofTranslationController : MonoBehaviour
    {

        public Text identity_proof,
            note1,
            note2,
            note3,
            identity_front,
            identity_back,
            identity_front_text,
            identity_back_text,
            upload_passport,
            waiting_identity_text_front, waiting_identity_text_back,
            verified_identity_front, verified_identity_back;

        public Text passport_proof,
                     passport,
                     passport_text,
                     upload_identity,
                     waiting_passport_text_front,
                     verified_passport_front;

        public Text address_proof,
                     residency,
                     residency_text,
                     waiting_residency_text_front,
                     verified_residency_front;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager._instance.scene = "IDProof";
            //ID VIEW
            identity_proof.text = TranslationManager._instance.Get("identity_proof") != string.Empty ? TranslationManager._instance.Get("identity_proof") : identity_proof.text;
            note1.text = TranslationManager._instance.Get("note1") != string.Empty ? TranslationManager._instance.Get("note1") : note1.text;
            note2.text = TranslationManager._instance.Get("note2") != string.Empty ? TranslationManager._instance.Get("note2") : note2.text;
            note3.text = TranslationManager._instance.Get("note3") != string.Empty ? TranslationManager._instance.Get("note3") : note3.text;
            identity_front.text = TranslationManager._instance.Get("identity_front") != string.Empty ? TranslationManager._instance.Get("identity_front") : identity_front.text;
            identity_back.text = TranslationManager._instance.Get("identity_back") != string.Empty ? TranslationManager._instance.Get("identity_back") : identity_back.text;
            identity_front_text.text = TranslationManager._instance.Get("identity_front_text") != string.Empty ? TranslationManager._instance.Get("identity_front_text") : identity_front_text.text;
            identity_back_text.text = TranslationManager._instance.Get("identity_back_text") != string.Empty ? TranslationManager._instance.Get("identity_back_text") : identity_back_text.text;
            upload_passport.text = TranslationManager._instance.Get("upload_passport") != string.Empty ? TranslationManager._instance.Get("upload_passport") : upload_passport.text;
            waiting_identity_text_front.text = TranslationManager._instance.Get("waiting_text") != string.Empty ? TranslationManager._instance.Get("waiting_text") : waiting_identity_text_front.text;
            waiting_identity_text_back.text = TranslationManager._instance.Get("waiting_text") != string.Empty ? TranslationManager._instance.Get("waiting_text") : waiting_identity_text_back.text;
            verified_identity_front.text = TranslationManager._instance.Get("verified") != string.Empty ? TranslationManager._instance.Get("verified") : verified_identity_front.text;
            verified_identity_back.text = TranslationManager._instance.Get("verified") != string.Empty ? TranslationManager._instance.Get("verified") : verified_identity_back.text;
            //PASSPORT VIEW
            passport_proof.text = TranslationManager._instance.Get("passport_proof") != string.Empty ? TranslationManager._instance.Get("passport_proof") : passport_proof.text;
            passport.text = TranslationManager._instance.Get("passport") != string.Empty ? TranslationManager._instance.Get("passport") : passport.text;
            passport_text.text = TranslationManager._instance.Get("passport_text") != string.Empty ? TranslationManager._instance.Get("passport_text") : passport_text.text;
            upload_identity.text = TranslationManager._instance.Get("upload_identity_card") != string.Empty ? TranslationManager._instance.Get("upload_identity_card") : upload_identity.text;
            waiting_passport_text_front.text = TranslationManager._instance.Get("waiting_text") != string.Empty ? TranslationManager._instance.Get("waiting_text") : waiting_passport_text_front.text;
            verified_passport_front.text = TranslationManager._instance.Get("verified") != string.Empty ? TranslationManager._instance.Get("verified") : identity_back.text;
            //RESIDENCY VIEW
            address_proof.text = TranslationManager._instance.Get("address_proof") != string.Empty ? TranslationManager._instance.Get("address_proof") : address_proof.text;
            residency.text = TranslationManager._instance.Get("residency") != string.Empty ? TranslationManager._instance.Get("residency") : residency.text;
            residency_text.text = TranslationManager._instance.Get("residency_text") != string.Empty ? TranslationManager._instance.Get("residency_text") : residency_text.text;
            waiting_residency_text_front.text = TranslationManager._instance.Get("waiting_text") != string.Empty ? TranslationManager._instance.Get("waiting_text") : waiting_passport_text_front.text;
            verified_residency_front.text = TranslationManager._instance.Get("verified") != string.Empty ? TranslationManager._instance.Get("verified") : identity_back.text;
        }
    }
}

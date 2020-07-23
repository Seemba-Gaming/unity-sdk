using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class SignupPresenter : MonoBehaviour
{
    public InputField username, email, password, confirmPassword;
    public Button Signup;
    string[] files;
    string pathPreFix;
    Image PlayerAvatar;
    //public GoogleAnalyticsV4 googleAnalytics;
    //public	Sprite sprite1,sprite2,sprite3,sprite4,sprite5,sprite6,sprite7,sprite8,sprite9,sprite10,sprite11,sprite12,sprite13,sprite14,sprite15,sprite16,sprite17,sprite18,sprite19,sprite20;
    public Sprite[] spriteArray;
    public Button changeCharacter;
    public GameObject popupChangeCharacter;
    int RandomValue;
    private bool isEmailValid, isUsernameValid, isPasswordValid, isPasswordConfirmed;
    // Use this for initialization
    void Start()
    {



        changeCharacter.onClick.AddListener(() =>
        {

            EventsController nbs = new EventsController();
            for (int i = 0; i <= 19; i++)
            {
                Image toggle = GameObject.Find("Toggle (" + i + ")").GetComponent<Image>();
                toggle.sprite = spriteArray[i];
                if (i == RandomValue)
                {
                    Toggle selectedToggle = GameObject.Find("Toggle (" + i + ")").GetComponent<Toggle>();
                    selectedToggle.isOn = true;
                }
            }
            nbs.ShowPopup("popup");
        });
        try
        {
            try
            {
                System.Random rnd = new System.Random();
                RandomValue = rnd.Next(0, 20);
                PlayerAvatar = GameObject.Find("Avatar").GetComponent<Image>();
                PlayerAvatar.sprite = spriteArray[RandomValue];
            }
            catch (NullReferenceException ex)
            {
                //Debug.Log ("AvatarException in username");
            }
        }
        catch (ArgumentNullException ex)
        {
        }
        UserManager um = new UserManager();
        username.onValueChanged.AddListener(delegate
        {
            GameObject.Find("LoaderUsername").GetComponent<Image>().transform.localScale = Vector3.one;
            GameObject.Find("AcceptedUsername").GetComponent<Image>().transform.localScale = Vector3.zero;
            GameObject.Find("DeclinedUsername").GetComponent<Image>().transform.localScale = Vector3.zero;
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                bool valide = um.checkUsername(username.text.ToUpper());
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    if (valide && username.text.Length >= 3)
                    {
                        GameObject.Find("AcceptedUsername").GetComponent<Image>().transform.localScale = Vector3.one;
                        GameObject.Find("DeclinedUsername").GetComponent<Image>().transform.localScale = Vector3.zero;
                        GameObject.Find("LoaderUsername").GetComponent<Image>().transform.localScale = Vector3.zero;
                        isUsernameValid = true;
                    }
                    else
                    {
                        GameObject.Find("AcceptedUsername").GetComponent<Image>().transform.localScale = Vector3.zero;
                        GameObject.Find("DeclinedUsername").GetComponent<Image>().transform.localScale = Vector3.one;
                        GameObject.Find("LoaderUsername").GetComponent<Image>().transform.localScale = Vector3.zero;
                        isUsernameValid = false;

                    }
                });
            });
        });
        email.onValueChanged.AddListener(delegate
        {
            GameObject.Find("LoaderEmail").GetComponent<Image>().transform.localScale = Vector3.one;
            GameObject.Find("AcceptedEmail").GetComponent<Image>().transform.localScale = Vector3.zero;
            GameObject.Find("DeclinedEmail").GetComponent<Image>().transform.localScale = Vector3.zero;
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                bool valide = um.checkMail(email.text);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    if (valide && IsValidEmail(email.text))
                    {
                        GameObject.Find("AcceptedEmail").GetComponent<Image>().transform.localScale = Vector3.one;
                        GameObject.Find("DeclinedEmail").GetComponent<Image>().transform.localScale = Vector3.zero;
                        GameObject.Find("LoaderEmail").GetComponent<Image>().transform.localScale = Vector3.zero;
                        isEmailValid = true;

                    }
                    else
                    {
                        GameObject.Find("AcceptedEmail").GetComponent<Image>().transform.localScale = Vector3.zero;
                        GameObject.Find("DeclinedEmail").GetComponent<Image>().transform.localScale = Vector3.one;
                        GameObject.Find("LoaderEmail").GetComponent<Image>().transform.localScale = Vector3.zero;
                        isEmailValid = false;

                    }
                });
            });
        });
        password.onValueChanged.AddListener(delegate
        {
            isPasswordConfirmed = false;
            isPasswordValid = true;
        });
        confirmPassword.onValueChanged.AddListener(delegate
        {

        });
    }

    bool IsValidEmail(string email)
    {
        string expresion;
        expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (Regex.IsMatch(email, expresion))
        {
            if (Regex.Replace(email, expresion, string.Empty).Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (popupChangeCharacter.transform.localScale == Vector3.one)
        {
            for (int counter = 0; counter <= 19; counter++)
            {
                Toggle character = GameObject.Find("Toggle (" + counter + ")").GetComponent<Toggle>();
                if (character.isOn)
                {
                    PlayerAvatar.sprite = spriteArray[counter];
                    RandomValue = counter;
                }
            }
        }

        if (isEmailValid && isUsernameValid && isPasswordValid && isPasswordConfirmed)
        {
            Signup.interactable = true;
        }
        else Signup.interactable = false;




        if (confirmPassword.text == password.text)
        {
            GameObject.Find("AcceptedConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.one;
            GameObject.Find("DeclinedConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.zero;
            GameObject.Find("LoaderConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.zero;
            isPasswordConfirmed = true;
        }
        else
        {
            GameObject.Find("AcceptedConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.zero;
            GameObject.Find("DeclinedConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.one;
            GameObject.Find("LoaderConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.zero;
            isPasswordConfirmed = false;
        }
        if (String.IsNullOrEmpty(confirmPassword.text))
        {
            isPasswordConfirmed = false;
            GameObject.Find("LoaderConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.zero;
            GameObject.Find("AcceptedConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.zero;
            GameObject.Find("DeclinedConfirmPassword").GetComponent<Image>().transform.localScale = Vector3.zero;
        }


    }
}

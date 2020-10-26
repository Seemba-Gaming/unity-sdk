using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class PersonelInfoController : MonoBehaviour
{
    #region Script Parameters
    public InputField LastName;
    public InputField FirstName;
    public InputField Address;
    public InputField City;
    public InputField ZipCod;
    public InputField Country;
    public InputField Personel_id_number;
    public InputField Phone;
    public Text Birthdate;
    public Text PlaceHolderAge;
    public Text country_phone_prefix;
    public Text country_code;
    public Image country_flag;
    public Button Age;
    public GameObject Phone_Containter;
    public GameObject Age_Containter;
    #endregion

    #region Fields
    private string _selectedDateString2 = "1995-09-15";
    private string token = null;
    #endregion

    #region Unity Methods
    public async void OnEnable()
    {
        token = UserManager.Get.getCurrentSessionToken();
        var usr = UserManager.Get.CurrentUser;
        if (usr != null)
        {
            string prefix = PhonePrefix.getPhonePrefix(usr.country_code.ToUpper());
            var mTexture = await UserManager.Get.GetFlagBytes(usr.country_code);
            country_phone_prefix.text = "(" + prefix + ")";
            country_code.text = usr.country_code.ToUpper();
            Sprite newSprite = null;
            try
            {
                newSprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
                country_flag.sprite = newSprite;
                country_flag.transform.localScale = Vector3.one;
            }
            catch (ArgumentNullException) { }
            if (!string.IsNullOrEmpty(usr.birthday))
            {
                Birthdate.text = DateTime.Parse(usr.birthday).ToString("yyyy-MM-dd");
                PlaceHolderAge.transform.localScale = Vector3.zero;
                Age.interactable = false;
                Age.GetComponent<InputfieldStateController>().ShowEditable();
            }
            else
            {
                PlaceHolderAge.transform.localScale = Vector3.one;
            }
            if (usr.country_code.ToLower().Equals("us"))
            {
                Personel_id_number.gameObject.SetActive(true);
            }

            if (!string.IsNullOrEmpty(usr.lastname))
            {
                LastName.text = usr.lastname;
                LastName.GetComponent<InputfieldStateController>().ShowEditable();
                LastName.readOnly = true;
            }
            if (!string.IsNullOrEmpty(usr.firstname))
            {
                FirstName.text = usr.firstname;
                FirstName.GetComponent<InputfieldStateController>().ShowEditable();
                FirstName.readOnly = true;
            }
            if (!string.IsNullOrEmpty(usr.adress))
            {
                Address.text = usr.adress;
                Address.GetComponent<InputfieldStateController>().ShowEditable();
                Address.readOnly = true;
            }
            if (!string.IsNullOrEmpty(usr.city))
            {
                City.text = usr.city;
                City.GetComponent<InputfieldStateController>().ShowEditable();
                City.readOnly = true;
            }
            if (!string.IsNullOrEmpty(usr.zipcode))
            {
                ZipCod.text = usr.zipcode.ToString();
                ZipCod.GetComponent<InputfieldStateController>().ShowEditable();
                ZipCod.readOnly = true;
            }
            if (!string.IsNullOrEmpty(usr.country))
            {
                Country.text = usr.country;
                Country.GetComponent<InputfieldStateController>().ShowEditable();
                Country.readOnly = true;
            }
            if (!string.IsNullOrEmpty(usr.phone))
            {
                Phone.text = usr.phone.Substring(prefix.Length);
                Phone.GetComponent<InputfieldStateController>().ShowEditable();
                Phone.readOnly = true;
            }
            Phone.onEndEdit.AddListener(async delegate
            {
                if (Phone.text != "" && Phone.text != usr.phone)
                {
                    string formatedPhone = prefix + Phone.text;
                    Debug.Log("formatedPhone: " + formatedPhone);
                    string[] attrib = {
                            "phone"
                    };
                    string[] value = {
                            formatedPhone
                    };

                    Phone.GetComponent<InputfieldStateController>().ShowLoading();
                    UpdateUser(attrib, value);
                    Phone.GetComponent<InputfieldStateController>().ShowAccepted();
                }
            });
            Phone.onValueChanged.AddListener(delegate
            {
                if (Phone_Containter.GetComponent<Animator>().GetBool("invalid") == true)
                {
                    Phone_Containter.GetComponent<Animator>().SetBool("invalid", false);
                }
            });
            LastName.onEndEdit.AddListener(async delegate
            {
                if (LastName.text != "" && LastName.text != usr.lastname)
                {
                    string[] attrib = {
                            "lastname"
                };
                    string[] value = {
                            LastName.text
                };
                    LastName.GetComponent<InputfieldStateController>().ShowLoading();
                    UpdateUser(attrib, value);
                    LastName.GetComponent<InputfieldStateController>().ShowAccepted();
                }
                else
                {
                    LastName.text = usr.lastname;
                }
            });
            FirstName.onEndEdit.AddListener(async delegate
            {
                if (FirstName.text != "" && FirstName.text != usr.firstname)
                {
                    string[] attrib = {"firstname"};
                    string[] value = { FirstName.text };
                    FirstName.GetComponent<InputfieldStateController>().ShowLoading();
                    UpdateUser(attrib, value);
                    FirstName.GetComponent<InputfieldStateController>().ShowAccepted();
                }
                else
                {
                    FirstName.text = usr.firstname;
                }
            });
            Address.onEndEdit.AddListener(async delegate
            {
                if (Address.text != "" && Address.text != usr.adress)
                {
                    string[] attrib = {
                            "address"
                    };
                    string[] value = {
                            Address.text
                    };
                    Address.GetComponent<InputfieldStateController>().ShowLoading();
                    UpdateUser(attrib, value);
                    Address.GetComponent<InputfieldStateController>().ShowAccepted();
                }
                else
                {
                    Address.text = usr.adress;
                }
            });
            City.onEndEdit.AddListener(async delegate
            {
                if (City.text != "" && City.text != usr.city)
                {
                    string[] attrib = {
                            "city"
                    };
                    string[] value = {
                            City.text
                    };
                    City.GetComponent<InputfieldStateController>().ShowLoading();
                    UpdateUser(attrib, value);
                    City.GetComponent<InputfieldStateController>().ShowAccepted();
                }
                else
                {
                    City.text = usr.city;
                }
            });
            ZipCod.onEndEdit.AddListener(async delegate
            {
                if (!ZipCod.text.Equals("0") && !ZipCod.text.Equals(usr.zipcode))
                {
                    string[] attrib = {
                            "zipcode"
                    };
                    string[] value = {
                            ZipCod.text
                    };
                    ZipCod.GetComponent<InputfieldStateController>().ShowLoading();
                    UpdateUser(attrib, value);
                    ZipCod.GetComponent<InputfieldStateController>().ShowAccepted();
                }
                else
                {
                    if (usr.zipcode.ToString() != "0")
                    {
                        ZipCod.text = usr.zipcode.ToString();
                    }
                }
            });
            Country.onEndEdit.AddListener(async delegate
            {
                if (Country.text != "" && Country.text != usr.country)
                {
                    string[] attrib = {
                            "country"
                };
                    string[] value = {
                            Country.text
                };
                    Country.GetComponent<InputfieldStateController>().ShowLoading();
                    UnityThreadHelper.CreateThread(() =>
                    {
                        UpdateUser(attrib, value);
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            Country.GetComponent<InputfieldStateController>().ShowAccepted();
                        });
                    });
                }
                else
                {
                    Country.text = usr.country;
                }
            });
            Personel_id_number.onEndEdit.AddListener(delegate
            {
                if (Personel_id_number.text != "" && Personel_id_number.text != usr.personal_id_number)
                {
                    string[] attrib = {
                            "personel_id_number"
                };
                    string[] value = {
                            Personel_id_number.text
                };
                    UpdateUser(attrib, value);
                }
                else
                {
                    Personel_id_number.text = usr.personal_id_number;
                }
            });
        }
        else
        {
            ConnectivityController.CURRENT_ACTION = ConnectivityController.PERSONNEL_INFO_ACTION;
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
        }
    }
    private void Update()
    {
        country_flag.rectTransform.sizeDelta = new Vector2(25f, country_flag.rectTransform.sizeDelta.y);
    }
    #endregion

    #region Methods
    public bool IsValidPhone(string Phone)
    {
        try
        {
            if (string.IsNullOrEmpty(Phone))
            {
                return false;
            }

            var r = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            return r.IsMatch(Phone);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async void UpdateUser(string[] attrib, string[] values)
    {
        UserManager.Get.UpdateUserByField(attrib, values);
    }

    private void OnDateCanceled()
    {
        Debug.Log("OnDateCanceled ");
        SelectedDateString2 = DateTime.Now.ToString("yyyy-MM-dd");
        PopupManager.Get.PopupViewPresenter.PopupAgeconfirmButton.interactable = false;
        PopupManager.Get.PopupViewPresenter.PopupAgePlaceHolder.text = "Select Date";
    }
    private void OnDateSelected(long val)
    {
        Debug.Log("OnDateSelected " + val);

        SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
        PopupManager.Get.PopupViewPresenter.PopupAgePlaceHolder.text = SelectedDateString2;
        PopupManager.Get.PopupViewPresenter.PopupAgeconfirmButton.interactable = true;
    }

    public void showExpPicker(UnityEngine.Object button)
    {
        if (Age_Containter.GetComponent<Animator>().GetBool("young") == true)
        {
            Age_Containter.GetComponent<Animator>().SetBool("young", false);
        }
        Debug.Log("showExpPicker");

        NativePicker.Instance.ShowDatePicker(GetScreenRect(button as GameObject), NativePicker.DateTimeForDate(2012, 12, 23), (long val) =>
        {
            SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
            Birthdate.text = SelectedDateString2;
            string[] attrib = { "birthdate" };
            string[] values = { Birthdate.text };
            string[] date = Birthdate.text.Split(new char[] { '-' }, 3);
            string Years = date[0];
            string Days = date[2];
            string Months = date[1];
            Debug.Log("Years: " + Years);
            if (DateTime.UtcNow.Year - int.Parse(Years) >= 18)
            {
                UnityThreadHelper.CreateThread(async () =>
                {
                    UpdateUser(attrib, values);
                });
            }
            else
            {
                // Age <18 : Show Error Text
                Age_Containter.GetComponent<Animator>().SetBool("young", true);
            }
        }, () =>
        {
            //SelectedDateString2 = DateTime.Now.ToString("yyyy-MM-dd");
        });
    }
    #endregion
    #region Implementations
    private Rect GetScreenRect(GameObject gameObject)
    {
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
    private String SelectedDateString2
    {
        get => _selectedDateString2;
        set
        {
            _selectedDateString2 = value;

            Birthdate.text = SelectedDateString2;
            PlaceHolderAge.transform.localScale = Vector3.zero;

        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
public class PersonelInfoController : MonoBehaviour
{
    public InputField LastName, FirstName, Address, City, ZipCode, Country, Personel_id_number, Phone;
    public Text Birthdate, PlaceHolderAge;
    public Text country_phone_prefix, country_code;
    public Image country_flag;
    private UserManager um = new UserManager();
    private InfoPersonelManager ipm = new InfoPersonelManager();
    public Button Age;
    private string _selectedDateString2 = "1995-09-15";
    string userId = null; string token = null;
    public static PersonelInfoController _Instance;
    public GameObject Phone_Containter, Age_Containter;
    // Use this for initialization
    public static PersonelInfoController getInstance()
    {
        return _Instance;
    }
    public async void OnEnable()
    {
        _Instance = this;
        try
        {
            SceneManager.UnloadSceneAsync("ConnectionFailed");
        }
        catch (ArgumentException ex) { }
        User usr;
        userId = um.getCurrentUserId();
        token = um.getCurrentSessionToken();
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreadHelper.CreateThread(() =>
        {
            usr = um.getUser(userId, token);
            UnityThreadHelper.Dispatcher.Dispatch(async () =>
            {
                SceneManager.UnloadSceneAsync("Loader");
                if (usr != null)
                {
                    string prefix = PhonePrefix.getPhonePrefix(usr.country_code.ToUpper());
                    string flagBytesString = um.GetFlagByte(usr.country_code);
                    country_phone_prefix.text = "(" + prefix + ")";
                    country_code.text = usr.country_code.ToUpper();
                    Texture2D txt = new Texture2D(1, 1);
                    Sprite newSprite = null;
                    try
                    {
                        Byte[] img = Convert.FromBase64String(flagBytesString);
                        txt.LoadImage(img);
                        newSprite = Sprite.Create(txt as Texture2D, new Rect(0f, 0f, txt.width, txt.height), Vector2.zero);
                        country_flag.sprite = newSprite;
                        country_flag.transform.localScale = Vector3.one;
                    }
                    catch (ArgumentNullException ex){}
                    if (!string.IsNullOrEmpty(usr.birthday))
                    {
                        Birthdate.text = DateTime.Parse(usr.birthday).ToString("yyyy-MM-dd");
                        PlaceHolderAge.transform.localScale = Vector3.zero;
                        Age.interactable = false;
                        GameObject.Find("editable birthday").transform.localScale = Vector3.zero;
                    }
                    else
                    {
                        PlaceHolderAge.transform.localScale = Vector3.one;
                    }
                    if (usr.country_code.ToLower().Equals("us"))
                    {
                        Personel_id_number.gameObject.SetActive(true);
                    }
                    if (!string.IsNullOrEmpty(usr.personal_id_number))
                    {
                        Personel_id_number.text = usr.personal_id_number;
                        Personel_id_number.readOnly = true;
                        GameObject.Find("editable Personal_id_number").transform.localScale = Vector3.zero;
                    }
                    if (!string.IsNullOrEmpty(usr.lastname))
                    {
                        LastName.text = usr.lastname;
                        GameObject.Find("editable lastname").transform.localScale = Vector3.zero;
                        LastName.readOnly = true;
                    }
                    if (!string.IsNullOrEmpty(usr.firstname))
                    {
                        FirstName.text = usr.firstname;
                        GameObject.Find("editable firstname").transform.localScale = Vector3.zero;
                        FirstName.readOnly = true;
                    }
                    if (!string.IsNullOrEmpty(usr.adress))
                    {
                        Address.text = usr.adress;
                        GameObject.Find("editable adress").transform.localScale = Vector3.zero;
                        Address.readOnly = true;
                    }
                    if (!string.IsNullOrEmpty(usr.city))
                    {
                        City.text = usr.city;
                        GameObject.Find("editable city").transform.localScale = Vector3.zero;
                        City.readOnly = true;
                    }
                    if (!string.IsNullOrEmpty(usr.zipcode))
                    {
                        ZipCode.text = usr.zipcode.ToString();
                        GameObject.Find("editable zipCode").transform.localScale = Vector3.zero;
                        ZipCode.readOnly = true;
                    }
                    if (!string.IsNullOrEmpty(usr.country))
                    {
                        Country.text = usr.country;
                        GameObject.Find("editable country").transform.localScale = Vector3.zero;
                        Country.readOnly = true;
                    }
                    if (!string.IsNullOrEmpty(usr.phone))
                    {
                        Phone.text = usr.phone.Substring(prefix.Length);
                        GameObject.Find("editable phone").transform.localScale = Vector3.zero;
                        Phone.readOnly = true;
                    }
                    Phone.onEndEdit.AddListener(async delegate
                    {
                        if (Phone.text != "" && Phone.text != usr.phone)
                        {
                            string formatedPhone =  prefix + Phone.text;
                            Debug.Log("formatedPhone: " + formatedPhone);
                            string[] attrib = {
                            "phone"
                            };
                            string[] value = {
                            formatedPhone
                            };
                            show("Button_Phone", "Loader");
                            if (await updateStripeAccount("phone",formatedPhone))
                            {
                                updateUser(attrib, value);
                                show("Button_Phone", "Accepted");
                            }
                            else
                            {
                                show("Button_Phone", "Declined");
                                UnityThreadHelper.Dispatcher.Dispatch(() =>
                                {
                                    Phone_Containter.GetComponent<Animator>().SetBool("invalid", true);
                                });
                            }
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
                            show("Button_LastName", "Loader");
                            if (await updateStripeAccount("lastname", LastName.text))
                            {
                                updateUser(attrib, value);
                                show("Button_LastName", "Accepted");
                            }
                            else
                            {
                                show("Button_LastName", "Declined");
                            }
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
                            string[] attrib = {
                            "firstname"
                        };
                            string[] value = {
                            FirstName.text
                        };
                            show("Button_FirstName", "Loader");
                            if (await updateStripeAccount("firstname", FirstName.text))
                            {
                                updateUser(attrib, value);
                                show("Button_FirstName", "Accepted");
                            }
                            else
                            {
                                show("Button_FirstName", "Declined");
                            }
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
                            show("Button_Adress", "Loader");
                            if (await updateStripeAccount("address", Address.text))
                            {
                                updateUser(attrib, value);
                                show("Button_Adress", "Accepted");
                            }
                            else
                            {
                                show("Button_Adress", "Declined");
                            }
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
                            show("Button_City", "Loader");
                            if (await updateStripeAccount("city", City.text))
                            {
                                updateUser(attrib, value);
                                show("Button_City", "Accepted");
                            }
                            else
                            {
                                show("Button_City", "Declined");
                            }
                        }
                        else
                        {
                            City.text = usr.city;
                        }
                    });
                    ZipCode.onEndEdit.AddListener(async delegate
                    {
                        if (ZipCode.text.ToString() != "0" && ZipCode.text != usr.zipcode.ToString())
                        {
                            string[] attrib = {
                            "zipcode"
                        };
                            string[] value = {
                            ZipCode.text
                        };
                            show("Button_ZipCode", "Loader");
                            if (await updateStripeAccount("zipcode", ZipCode.text))
                            {
                                updateUser(attrib, value);
                                show("Button_ZipCode", "Accepted");
                            }
                            else
                            {
                                show("Button_ZipCode", "Declined");
                            }
                        }
                        else
                        {
                            if (usr.zipcode.ToString() != "0")
                                ZipCode.text = usr.zipcode.ToString();
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
                            show("Button_Country", "Loader");
                            UnityThreadHelper.CreateThread(() =>
                            {
                                updateUser(attrib, value);
                                UnityThreadHelper.Dispatcher.Dispatch(() =>
                                {
                                    show("Button_Country", "Accepted");
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
                            updateUser(attrib, value);
                        }
                        else
                        {
                            Personel_id_number.text = usr.personal_id_number;
                        }
                    });
                }
                else
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        try
                        {
                            SceneManager.UnloadSceneAsync("ConnectionFailed");
                        }
                        catch (ArgumentException ex) { }
                        ConnectivityController.CURRENT_ACTION = ConnectivityController.PERSONNEL_INFO_ACTION;
                        SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                        try
                        {
                            SceneManager.UnloadSceneAsync("Loader");
                        }
                        catch (ArgumentException ex) { }
                    });
                }
            });
        });
    }
    public void show(string path, string objectname)
    {
        hideOthers(path, objectname);
        GameObject.Find(path + "/" + objectname).transform.localScale = Vector3.one;
    }
    public void hideOthers(string path, string objectname)
    {
        switch (objectname)
        {
            case "Loader":
                hide(path, "Accepted");
                hide(path, "Declined");
                break;
            case "Accepted":
                hide(path, "Loader");
                hide(path, "Declined");
                break;
                break;
            case "Declined":
                hide(path, "Loader");
                hide(path, "Accepted");
                break;
        }
        GameObject.Find(path + "/" + objectname).transform.localScale = Vector3.zero;
    }
    public void hide(string path, string objectname)
    {
        GameObject.Find(path + "/" + objectname).transform.localScale = Vector3.zero;
    }
    public bool IsValidPhone(string Phone)
    {
        try
        {
            if (string.IsNullOrEmpty(Phone))
                return false;
            var r = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            return r.IsMatch(Phone);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void updateUser(string[] attrib, string[] values)
    {
        um.UpdateUserByField(userId, token, attrib, values);
    }
    public async Task<bool> updateStripeAccount(string key, string value)
    {
        bool success = false;
        WithdrawManager wm = new WithdrawManager();
        Debug.Log("Key:" + key);
        switch (key)
        {
            case "firstname":
                return await wm.attachInfoToAccount(token, value, "first_name");
                break;
            case "lastname":
                return await wm.attachInfoToAccount(token, value, "last_name");
                break;
            case "address":
                return await wm.attachInfoToAccount(token, value, "address", "line1");
                break;
            case "zipcode":
                return await wm.attachInfoToAccount(token, value, "address", "postal_code");
                break;
            case "city":
                return await wm.attachInfoToAccount(token, value, "address", "city");
                break;
            case "phone":
                return await wm.attachInfoToAccount(token, value, "phone");
                break;
            case "birthdate":
                Debug.Log("case birthdate");
                char[] spearator = { '-' };
                String[] birth = value.Split(spearator);
                return wm.attachDOBToAccount(token, int.Parse(birth[2]), int.Parse(birth[1]), int.Parse(birth[0]));
                break;
            default: return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        country_flag.rectTransform.sizeDelta = new Vector2(25f, country_flag.rectTransform.sizeDelta.y);
    }
    private String SelectedDateString2
    {
        get
        {
            return _selectedDateString2;
        }
        set
        {
            _selectedDateString2 = value;
            
            Birthdate.text = SelectedDateString2;
            PlaceHolderAge.transform.localScale = Vector3.zero;
            
        }
    }
    public void showExpPicker(UnityEngine.Object button)
    {
        if (Age_Containter.GetComponent<Animator>().GetBool("young") == true)
        {
            Age_Containter.GetComponent<Animator>().SetBool("young", false);
        }
        Birthdate = GameObject.Find("AgeInfoPers").GetComponent<Text>();
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
                    if (await updateStripeAccount("birthdate", Birthdate.text))
                        updateUser(attrib, values);
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
    private Rect GetScreenRect(GameObject gameObject)
    {
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
}

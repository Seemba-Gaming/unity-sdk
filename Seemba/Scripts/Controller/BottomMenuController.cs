using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Reflection;
public class BottomMenuController : MonoBehaviour 
{   private static BottomMenuController _Instance;
    public GameObject Toolbar { get { return GetComponent<GameObject>(); } }
    public GameObject home,haveFun,WinMoney,settings;
    public static int _currentPage = 0;
    public String selectedMenu="Home";
    public String menu
    {
        get { return selectedMenu; }
        set { selectedMenu = value; }
    }
    void Start() {
        _Instance=this;
        InvokeRepeating("hideOrShowToolbar", 0f, 0.01f);
    }
    public static void Hide() {

       try
        {
           GameObject.Find("Toolbar").transform.localScale = Vector3.zero;
        }
        catch (Exception e) {
        }
    }
    public static void Show() {
        
        try
        {
            GameObject.Find("Toolbar").transform.localScale = Vector3.one;
        }
        catch (Exception e) { }
    }
    public void hideOrShowToolbar() {
        try {
            if ((ScrollSnapRect.currentView=="Settings"&&ScrollSnapRect.focusedPage!=0)|| ScrollSnapRect.currentView == "Wallet") {
            GameObject.Find("Toolbar").transform.localScale = Vector3.zero;
            }
        else
        {  if (ScrollSnapRect.currentView != "WinMoney" && ScrollSnapRect.currentView != "HaveFun") { }
        }
        }catch(NullReferenceException ex) { }
    }
    public static BottomMenuController getInstance(){
        return _Instance;
    }
    public void selectHome(){
        home.GetComponent<Animator>().SetBool("focused", true);
        menu = "Home";
    }
    public void unselectHome(){
        if (menu == "Home") { home.GetComponent<Animator>().SetBool("focused", false); }
    }
    public void selectHaveFun(){
        haveFun.GetComponent<Animator>().SetBool("focused", true);
        menu = "HaveFun";
    }
    public void unselectHaveFun(){
        if (menu == "HaveFun") { haveFun.GetComponent<Animator>().SetBool("focused", false); }
    }
    public void selectWinMoney(){
        WinMoney.GetComponent<Animator>().SetBool("focused", true);
        menu = "WinMoney";
    }
    public void unselectWinMoney(){
        if (menu == "WinMoney") { WinMoney.GetComponent<Animator>().SetBool("focused", false); }
    }
    public void selectSettings(){
        settings.GetComponent<Animator>().SetBool("focused", true);
        menu = "Settings";
    }
    public void unselectSettings(){
        if (menu == "Settings") { settings.GetComponent<Animator>().SetBool("focused", false); }
    }
}
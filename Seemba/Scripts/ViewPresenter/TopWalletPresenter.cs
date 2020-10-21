using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Timers;
using System.Linq;

public class TopWalletPresenter : MonoBehaviour {
	private static TopWalletPresenter _Instance; 
    public GameObject remove_fee_prefab;
    public GameObject remove_fee_container;
    void OnEnable() {
        _Instance=this;
    }
	public  static TopWalletPresenter getInstance(){
         return _Instance;
    }
	public void remove_fees(float fee){
        Debug.Log("start duplicating...");
        GameObject remove_fee = Instantiate(remove_fee_prefab) as GameObject;
        remove_fee.transform.SetParent (remove_fee_container.transform, false);
        Animator remove_fee_animator=remove_fee.GetComponent<Animator>();
        Text remove_fee_text=remove_fee.GetComponent<Text>();
        remove_fee_text.text="-" + fee.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        remove_fee_animator.SetTrigger("show");
        StartCoroutine(animation_finished(remove_fee_animator, remove_fee));
    }
    public void add_amount(float amount){
        
    }

    IEnumerator animation_finished(Animator animator,GameObject obj){
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1){
                Debug.Log("animation playing...");
                yield return new WaitForSeconds(1f);
        }

        Debug.Log("animation finished ---> Destroy");
        Destroy(obj);
 
    }
	
}
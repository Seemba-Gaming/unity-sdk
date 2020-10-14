using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NotificationController : MonoBehaviour
{
    // Start is called before the first frame update
  /*  void Start()
    {
        Debug.Log("Notification");
    Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived; 
    Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
}
public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
  UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
  PlayerPrefs.SetString("DeviceToken",token.Token);
}
public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
  UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
  //Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(0), e.Message.Notification.Title, e.Message.Notification.Body, new Color(0, 0.6f, 1), Assets.SimpleAndroidNotifications.NotificationIcon.Event);
}
    // Update is called once per frame
    void Update()
    {
    }
  */
}

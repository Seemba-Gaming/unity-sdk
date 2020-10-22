using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentry;
using UnityEngine;

class SentryController: MonoBehaviour
{
    private static SentryController _Instance = null;
    private bool isInstantiated = false;
    private SentryController()
    {
    }

    public static SentryController Instance
    {   
        get{
            if (_Instance == null)
            {   
                _Instance = new SentryController(); 
            }
            return _Instance;
         }
    }

    

    public void instantiate()
    {
        if (!isInstantiated)
        {
            var sentry = new GameObject("Sentry").AddComponent(typeof(SentrySdk)) as SentrySdk;
            sentry.Dsn = "https://7311304e2e7242c78ce92b23476e3b35@crash.seemba.com/2";
            sentry.Version = "0.0.1";
            isInstantiated = true;
        }
    }
    public void exc(){
        throw new NullReferenceException();
    }
    private new void SendMessage(string message)
    {
        
        if (message == "event")
        {
            var @event = new SentryEvent("Event message")
            {
                level = "debug"
            };

            SentrySdk.CaptureEvent(@event);
        }
    }

}


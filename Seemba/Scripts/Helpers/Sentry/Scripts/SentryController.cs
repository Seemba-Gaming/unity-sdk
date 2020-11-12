using System;
using Sentry;
using UnityEngine;

[CLSCompliant(false)]
#pragma warning disable CS3019 // La vérification de conformité CLS ne sera pas effectuée, car l'objet inspecté n'est pas visible hors de cet assembly
class SentryController
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
    private void SendMessage(string message)
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
#pragma warning restore CS3019 // La vérification de conformité CLS ne sera pas effectuée, car l'objet inspecté n'est pas visible hors de cet assembly



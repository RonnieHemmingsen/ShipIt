using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StringEvent : UnityEvent<string>
{
    
}

public class EventManager : MonoBehaviour {

    private Dictionary<string, StringEvent> stringEventDictionary;
    private Dictionary<string, UnityEvent> eventDictionary;
    private static EventManager _eventManager;

    public static EventManager instance
    {
        get
        {
            if(!_eventManager)
            {
                _eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;
            }

            if(!_eventManager)
            {
                Debug.LogError("EventManager missing");
            }
            else
            {
                _eventManager.Init();
            }

            return _eventManager;
        }
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }

        if(stringEventDictionary == null)
        {
            stringEventDictionary = new Dictionary<string, StringEvent>();
        }
    }

    #region "Standard Event"
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);   
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if(_eventManager == null) return;

        UnityEvent thisEvent = null;

        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, string param = "")
    {
        UnityEvent thisEvent = null;
        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
    #endregion

    #region "StringEvent"
    public static void StartListeningForStringEvent(string eventName, UnityAction<string> listener)
    {
        StringEvent thisEvent = null;
        if (instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);   
        }
        else
        {
            thisEvent = new StringEvent();
            thisEvent.AddListener(listener);
            instance.stringEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListeningForStringEvent(string eventName, UnityAction<string> listener)
    {
        if(_eventManager == null) return;

        StringEvent thisEvent = null;

        if(instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerStringEvent(string eventName, string param)
    {
        StringEvent thisEvent = null;
        if(instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
            
    }
    #endregion



}

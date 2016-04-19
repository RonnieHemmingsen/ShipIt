using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ObjectEvent : UnityEvent<GameObject>
{}

[Serializable]
public class StringEvent : UnityEvent<string>
{}

[Serializable]
public class IntEvent : UnityEvent<int>
{}

public class EventManager : MonoBehaviour {

    private Dictionary<string, ObjectEvent> objectEventDictionary;
    private Dictionary<string, IntEvent> intEventDictionary;
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
                Debug.LogWarning("EventManager missing");
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

        if(intEventDictionary == null)
        {
            intEventDictionary = new Dictionary<string, IntEvent>();
        }

        if(objectEventDictionary == null)
        {
            objectEventDictionary = new Dictionary<string, ObjectEvent>();
        }
    }

    #region "Standard Event"
    public static void StartListening(string eventName, UnityAction listener)
    {
       
        UnityEvent thisEvent = null;
        try
        {
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
        } catch (Exception ex)
        {
            print("StartListening error: " + ex);
        }

    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if(_eventManager == null) return;

        UnityEvent thisEvent = null;

        try
        {
            if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        } catch (Exception ex)
        {
            print("StopListening error: " + ex);
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

    #region int event
    public static void StartListeningForIntEvent(string eventName, UnityAction<int> listener)
    {
        IntEvent thisEvent = null;
        if (instance.intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);   
        }
        else
        {
            thisEvent = new IntEvent();
            thisEvent.AddListener(listener);
            instance.intEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListeningForIntEvent(string eventName, UnityAction<int> listener)
    {
        if(_eventManager == null) return;

        IntEvent thisEvent = null;

        if(instance.intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerIntEvent(string eventName, int param)
    {
        IntEvent thisEvent = null;
        if(instance.intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }

    }
    #endregion

    #region "Object Event"
    public static void StartListeningForObjectEvent(string eventName, UnityAction<GameObject> listener)
    {
        ObjectEvent thisEvent = null;
        if (instance.objectEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);   
        }
        else
        {
            thisEvent = new ObjectEvent();
            thisEvent.AddListener(listener);
            instance.objectEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListeningForObjectEvent(string eventName, UnityAction<GameObject> listener)
    {
        if(_eventManager == null) return;

        ObjectEvent thisEvent = null;

        if(instance.objectEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerObjectEvent(string eventName, GameObject param)
    {
        ObjectEvent thisEvent = null;
        if(instance.objectEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }

    }
    #endregion



}

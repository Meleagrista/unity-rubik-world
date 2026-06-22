using System;
using System.Collections.Generic;
using UnityEngine;

public enum Event
{
    INVALID_EVENT = -1,
    CAMERA_LOCK_EVENT,
    CAMERA_UNLOCK_EVENT,
    PAWN_ANIMATION_EVENT,
    LEVEL_LOAD_EVENT,
    GAME_STARTED_EVENT,
    GAME_WIN_EVENT,
    GAME_LOSE_EVENT,
    GAME_PAUSE_EVENT,
    GAME_RESUME_EVENT,
}

public class EventManager : MonoBehaviour
{
    private Dictionary<Event, Action<Dictionary<string, object>>> eventDictionary;

    private static EventManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        eventDictionary = new Dictionary<Event, Action<Dictionary<string, object>>>();
        DontDestroyOnLoad(gameObject);
    }

    public static void StartListening(Event eventName, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> thisEvent;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Event eventName, Action<Dictionary<string, object>> listener)
    {
        if (instance == null) 
            return;

        Action<Dictionary<string, object>> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(Event eventName, Dictionary<string, object> message)
    {
        Action<Dictionary<string, object>> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }
}
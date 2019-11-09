using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();
    private List<Action> actions = new List<Action>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].OnEventRaised();
        }

        for (int i = actions.Count - 1; i >= 0; --i)
        {
            actions[i].Invoke();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void RegisterAction(Action action)
    {
        actions.Add(action);
    }

    public void UnregisterAction(Action action)
    {
        actions.Remove(action);
    }
}

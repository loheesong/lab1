using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent<T> : ScriptableObject
{
    private readonly List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();

    public void Raise(T data)
    {
        // Iterate backwards so unregistering inside callbacks doesn't skip listeners
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(data);
        }
    }

    public void RegisterListener(GameEventListener<T> listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener<T> listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
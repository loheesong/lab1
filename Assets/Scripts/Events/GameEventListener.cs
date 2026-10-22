using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<T> : MonoBehaviour
{
    [Header("Event to Listen to")]
    public GameEvent<T> gameEvent;

    [Header("Response")]
    public UnityEvent<T> response;

    protected virtual void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(this);
        }
    }

    protected virtual void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
        }
    }

    public void OnEventRaised(T data)
    {
        response?.Invoke(data);
    }
}
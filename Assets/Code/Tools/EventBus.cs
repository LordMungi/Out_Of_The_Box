
using System;
using System.Collections.Generic;

public class EventBus : IService
{
    public delegate void EventCallback<T>(in T eventData) where T : struct, IEvent;
    public bool IsPersistant => false;

    private ConcurrentPool _eventPool = new ConcurrentPool();
    private Dictionary<Type, List<Delegate>> _events = new Dictionary<Type, List<Delegate>>();

    public void Subscribe<T>(EventCallback<T> callback) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (!_events.ContainsKey(eventType))
            _events.Add(eventType, new List<Delegate>());

        _events[eventType].Add(callback);
    }

    public void Unsubscribe<T>(EventCallback<T> callback) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_events.TryGetValue(eventType, out List<Delegate> subscriptions))
            subscriptions.Remove(callback);
    }

    public void Raise<T>(params object[] parameters) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        T raisingEvent = _eventPool.Get<T>(parameters);

        if (_events.TryGetValue(eventType, out List<Delegate> subscriptions))
        {
            foreach (Delegate callback in subscriptions)
            {
                ((EventCallback<T>)callback)?.Invoke(raisingEvent);
            }
        }
        _eventPool.Release(raisingEvent);
    }

    public void Clear()
    {
        _events.Clear();
    }
}

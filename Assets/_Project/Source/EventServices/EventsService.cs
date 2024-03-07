using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//TODO: ainda melhorando esse service, ele tem um bug na "unsubscribe" e ainda precisa usar o "Unsubscribe" um por 1 manualmente no "dispose"
public class EventsService : MonoBehaviour, IEventsService
{
    private readonly Dictionary<Type, List<object>> _eventListeners = new();

    public void Subscribe<T>(Action<T> callback) where T : IEvent
    {
        Type eventType = typeof(T);

        if (!_eventListeners.ContainsKey(eventType))
        {
            _eventListeners[eventType] = new List<object>();
        }

        _eventListeners[eventType].Add(callback);
    }

    public void Unsubscribe<T>(Action<T> callback) where T : IEvent
    {
        Type eventType = typeof(T);

        if (_eventListeners.TryGetValue(eventType, out List<object> listener))
        {
            listener.Remove(callback);
        }
    }

    public void Invoke<T>(T eventData) where T : IEvent
    {
        Type eventType = typeof(T);

        if (_eventListeners.ContainsKey(eventType))
        {
            List<object> eventListeners = _eventListeners[eventType].ToList(); //TODO: melhorar isso para não deixar eventos que nao serão mais usados ocupando alocacao :/

            foreach (object handler in eventListeners)
            {
                if (handler is Action<T> castedHandler)
                {
                    castedHandler.Invoke(eventData);
                }
            }
        }
    }
}

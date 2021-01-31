using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ/Simple Event Channel")]
public abstract class GenericEventChannel<TValue> : ScriptableObject
{
    HashSet<Action<TValue>> _subscribers = new HashSet<Action<TValue>>();

    public bool AddListener(Action<TValue> action) => _subscribers.Add(action);
    public bool RemoveListener(Action<TValue> action) => _subscribers.Remove(action);
    
    public void Raise(TValue value)
    {
        foreach(var action in _subscribers)
        {
            action(value);
        }
    }
}


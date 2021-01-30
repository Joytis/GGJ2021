using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ/Simple Event Channel")]
public class SimpleEventChannel : ScriptableObject
{
    HashSet<Action> _subscribers = new HashSet<Action>();

    public bool AddListener(Action action) => _subscribers.Add(action);
    public bool RemoveListener(Action action) => _subscribers.Remove(action);
    public void Raise()
    {
        foreach(var action in _subscribers)
        {
            action();
        }
    }
}


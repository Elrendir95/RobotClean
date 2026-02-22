using UnityEngine;
using UnityEngine.Events;

public class BaseEventChannel : ScriptableObject
{
    public event UnityAction OnEventRaised;
    public void Raise() => OnEventRaised?.Invoke();
}

public class BaseEventChannel<T> : ScriptableObject
{
    public event UnityAction<T> OnEventRaised;
    public void Raise(T value) => OnEventRaised?.Invoke(value);
}

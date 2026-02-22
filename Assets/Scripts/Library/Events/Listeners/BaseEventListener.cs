using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// BaseEventListener, pour gerer les event channel sans paramettre
/// Et permettre de setter facilement depuis l'inspector des action a realiser lors du trigger
/// </summary>
public class BaseEventListener : MonoBehaviour
{
    [SerializeField] private BaseEventChannel eventChannel = default;
    public UnityEvent OnEventRaised;

    private void OnEnable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised += Respond;
        }
    }

    private void OnDisable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised -= Respond;
        }
    }

    private void Respond()
    {
        OnEventRaised?.Invoke();
    }
}


/// <summary>
/// BaseEventListener, pour gerer les event channel avec un paramettre
/// Et permettre de setter facilement depuis l'inspector des action a realiser lors du trigger
/// </summary>
public class BaseEventListener<T> : MonoBehaviour
{
    [SerializeField] private BaseEventChannel<T> eventChannel = default;
    public UnityEvent<T> OnEventRaised;

    private void OnEnable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised += Respond;
        }
    }

    private void OnDisable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised -= Respond;
        }
    }

    private void Respond(T value)
    {
        OnEventRaised?.Invoke(value);
    }
}

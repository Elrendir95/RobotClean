using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseVariable<T> : ScriptableObject
{
    [SerializeField] private T _value;

#if UNITY_EDITOR
    private T _savedValue;
    [SerializeField] private bool _registerInPlayMode = false;

    /// <summary>
    /// Met le SetDirty quand la valeur de la variable a changer
    /// </summary>
    /// <param name="arg0"></param>
    private void SetDirty(T arg0)
    {
        EditorUtility.SetDirty(this);
    }
#endif

    void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        onValueChanged.AddListener(SetDirty);
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
        onValueChanged.RemoveListener(SetDirty);
#endif
    }

#if UNITY_EDITOR
    private void EditorApplicationOnplayModeStateChanged(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.EnteredPlayMode)
        {
            _savedValue = _value;
        }
        else if (obj == PlayModeStateChange.ExitingPlayMode)
        {
            if (!_registerInPlayMode)
            {
                _value = _savedValue;
            }
        }
    }
#endif

    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                onValueChanged?.Invoke(_value);
            }
        }
    }

    public UnityEvent<T> onValueChanged;

    // Cette méthode est appelée par Unity quand un champ est modifié dans l'inspecteur
    private void OnValidate()
    {
        onValueChanged?.Invoke(_value);
    }
}

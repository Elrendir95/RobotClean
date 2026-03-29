using System;
using Components.AudioSystem;
using Components.EventSystem;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] private AudioSO collectSound;

    private void Start()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Collectable"))
        {
            Debug.LogError($"{gameObject.name} collectable not affected on the correct layer");
        }
    }

    public virtual void OnCollect(GameObject collector)
    {
        Events.PlayAudio?.Invoke(collectSound);
        Destroy(gameObject);
    }
}

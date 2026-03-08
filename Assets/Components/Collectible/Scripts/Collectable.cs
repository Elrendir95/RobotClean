using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Collectable"))
        {
            Debug.LogError($"{gameObject.name} collectable not affected on the correct layer");
        }
    }

    public virtual void OnCollect(GameObject collector)
    {
        Destroy(gameObject);
    }
}

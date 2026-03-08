using UnityEngine;

[RequireComponent(typeof(CollectableTrigger))]
public class TrashCollectable : Collectable
{
    public override void OnCollect(GameObject collector)
    {
        Debug.Log($"This {gameObject.name} collects a trash has been collected");
        Destroy(gameObject);
    }
}

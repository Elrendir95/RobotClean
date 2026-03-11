using Components.EventSystem;
using UnityEngine;

public class TrashCollectable : Collectable
{
    [SerializeField] private float lifeBonus = 4f;
    public override void OnCollect(GameObject collector)
    {
        Debug.Log($"This {gameObject.name} collects a trash has been collected");
        Events.UpdateLife(lifeBonus);
        Destroy(gameObject);
    }
}

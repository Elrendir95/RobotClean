using Components.EventSystem;
using UnityEngine;

public class TrashCollectable : Collectable
{
    [SerializeField] private float lifeBonus = 4f;
    public override void OnCollect(GameObject collector)
    {
        Events.UpdateLife(lifeBonus);
        Destroy(gameObject);
    }
}

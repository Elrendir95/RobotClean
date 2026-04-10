using Components.EventSystem;
using UnityEngine;

namespace Components.Collectible
{
    /// <summary>
    /// Standard Collectable Trash, that give back some life
    /// </summary>
    public class TrashCollectable : Collectable
    {
        [SerializeField] private float lifeBonus = 4f;

        public override void OnCollect(GameObject collector)
        {
            Events.UpdateLife?.Invoke(lifeBonus);
            base.OnCollect(collector);
        }
    }
}

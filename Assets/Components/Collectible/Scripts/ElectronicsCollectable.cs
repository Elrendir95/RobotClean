using Components.EventSystem;
using UnityEngine;

namespace Components.Collectible
{
    public class ElectronicsCollectable : Collectable
    {
        [SerializeField] private int electronicsComponents = 1;
        public override void OnCollect(GameObject collector)
        {
            Events.OnElectronicsCollected?.Invoke(electronicsComponents);
            base.OnCollect(collector);
        }
    }
}

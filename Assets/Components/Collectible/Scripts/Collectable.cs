using Components.AudioSystem;
using Components.EventSystem;
using UnityEngine;

namespace Components.Collectible
{
    /// <summary>
    /// Parent Class for all Object that are collectable
    /// </summary>
    public abstract class Collectable : MonoBehaviour
    {
        [SerializeField] private AudioSO collectSound;

        private void Start()
        {
            if (gameObject.layer != LayerMask.NameToLayer("Collectable"))
            {
                // Log and error if the Collectable is on the expected Layer
                Debug.LogError($"{gameObject.name} collectable not affected on the correct layer");
            }
        }

        /// <summary>
        /// Executed when collected by the "collector"
        /// </summary>
        /// <param name="collector">the GameObject that have collected the Collectable</param>
        public virtual void OnCollect(GameObject collector)
        {
            Events.PlayAudio?.Invoke(collectSound);
            Destroy(gameObject);
        }
    }
}

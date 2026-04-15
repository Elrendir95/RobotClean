using UnityEngine;

namespace Sections
{
    /// <summary>
    /// Deprecated, this was used in early builds to demonstrate to the GD
    /// </summary>
    public class SectionDestroyOnExit : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) Destroy(gameObject, .1f);
        }
    }
}

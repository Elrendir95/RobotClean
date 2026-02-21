using UnityEngine;

namespace Sections
{
    public class SectionDestroyOnExit : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) Destroy(gameObject, .1f);
        }
    }
}

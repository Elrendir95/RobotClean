using UnityEngine;

namespace Sections
{
    /// <summary>
    /// Deprecated, this was used in early builds to demonstrate to the GD
    /// </summary>
    public class MoveOnExit : MonoBehaviour
    {
        private int sectionsAway = 6;
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameObject.transform.Translate(Vector3.forward * GetComponent<BoxCollider>().bounds.extents.z * 2f * sectionsAway);
            }
        }
    }
}

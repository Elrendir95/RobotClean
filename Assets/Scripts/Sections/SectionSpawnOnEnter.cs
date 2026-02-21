using UnityEngine;
using UnityEngine.Serialization;

namespace Sections
{
    public class SectionSpawnOnEnter : MonoBehaviour
    {
        private bool _spawned = false;
        [SerializeField] private int sectionsAway = 6;
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_spawned)
            {
                Vector3 nextPosition = transform.position + new Vector3(0f, 0f, gameObject.GetComponent<BoxCollider>().bounds.extents.z * 2f * sectionsAway);
                Instantiate(gameObject, nextPosition, gameObject.transform.rotation);
                _spawned = true;
            }
        }
    }
}

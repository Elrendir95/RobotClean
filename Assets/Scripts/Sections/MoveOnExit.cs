using System;
using UnityEngine;

namespace Sections
{
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

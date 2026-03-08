using System;
using Components.EventSystem;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 sphereCenter;
        [SerializeField] private float sphereRadius;

        private bool _isHit = false;

        private Vector3 PlayerSpherePosition => transform.position + sphereCenter;

        private void Start()
        {
            EventSystem.OnPlayerSlideDown += OnPlayerSlideDown;
        }

        private void OnPlayerSlideDown(bool isSlidingDown)
        {

        }

        private void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, sphereRadius);
            if (hitColliders.Length > 0 && !_isHit)
            {
                _isHit = true;
                Debug.Log("Player Hit something");
            }
            if (hitColliders.Length == 0)
            {
                _isHit = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerSpherePosition, sphereRadius);
        }
#endif
    }
}

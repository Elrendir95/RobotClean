using Components.EventSystem;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 sphereCenter;
        [SerializeField, Tooltip("Obstacle sphere radius")] private float obstacleSphereRadius = 0.5f;
        [SerializeField, Tooltip("Collectable sphere radius")] private float collectableSphereRadius = 1f;
        [SerializeField] private LayerMask collectableLayer;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float obstacleDamaged = 25f;
        [SerializeField, Tooltip("Invincibility time after obstacle hits in seconds")]
        private float invincibilityTime = 1.5f;

        private bool _isInvincible = false;

        private Vector3 PlayerSpherePosition => transform.position + sphereCenter;

        private void Update()
        {
            CheckObstacle();
            CheckCollectable();
        }

        private void CheckCollectable()
        {
            Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, collectableSphereRadius, collectableLayer);
            foreach (Collider hitCollider in hitColliders)
            {
                hitCollider.gameObject.GetComponent<Collectable>().OnCollect(gameObject);
            }
        }

        private void CheckObstacle()
        {
            // If we are invincible do an early return
            if (_isInvincible) return;

            Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, obstacleSphereRadius, obstacleLayer);
            if (hitColliders.Length > 0)
            {
                Debug.Log("Player Hit something");
                Events.UpdateLife(-obstacleDamaged);
                StartCoroutine(InvincibilityCoroutine());
            }
        }

        IEnumerator InvincibilityCoroutine()
        {
            _isInvincible = true;
            yield return new WaitForSeconds(invincibilityTime);
            _isInvincible = false;
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerSpherePosition, obstacleSphereRadius);
            Gizmos.color = Color.lightGreen;
            Gizmos.DrawWireSphere(PlayerSpherePosition, collectableSphereRadius);
        }
#endif
    }
}

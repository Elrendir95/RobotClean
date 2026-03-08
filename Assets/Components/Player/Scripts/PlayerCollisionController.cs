using Components.EventSystem;
#if UNITY_EDITOR
using Player;
using UnityEditor;
#endif
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
            Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, obstacleSphereRadius, obstacleLayer);
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

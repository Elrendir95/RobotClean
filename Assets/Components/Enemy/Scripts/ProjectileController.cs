using System;
using Components.EventSystem;
using Library.References;
using UnityEngine;

namespace Components.Enemy
{
    public class ProjectileController : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private FloatReference globalSpeed;
        [SerializeField] private float projectileSpeed = 5f;

        private Vector3 _direction;
        private Vector3 _targetPosition;

        public void SetTarget(Vector3 target)
        {
            _direction = (target - transform.position).normalized;
            _targetPosition = target;
        }

        public void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            // Compute the distance that should be done this frame by
            // adding the current speed to the projectile speed
            // otherwise chunks could move faster than the projectile...
            float distanceThisFrame = (projectileSpeed + globalSpeed) * Time.deltaTime;

            float distanceRemaining = (_targetPosition - transform.position).magnitude;

            if (distanceRemaining <= distanceThisFrame)
            {
                // TODO add Events.PlayAudioAt()
                Destroy(gameObject);
                return;
            }

            // Actually move the projectile
            transform.Translate(_direction * distanceThisFrame, Space.World);
        }

    }
}

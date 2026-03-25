using System;
using Library.References;
using UnityEngine;

namespace Components.Enemy
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private FloatReference globalSpeed;
        [SerializeField] private float projectileSpeed = 5f;

        private Vector3 _direction;
        private Vector3 _targetPosition;
        private float _damage = 10f;

        public void SetTarget(Vector3 target)
        {
            _direction = (target - transform.position).normalized;
            _targetPosition = target;
        }

        public void SetDamage(float damage)
        {

        }

        public void Update()
        {
            float distanceThisFrame = (projectileSpeed + globalSpeed) * Time.deltaTime;
            float distanceRemaining = (_targetPosition - transform.position).magnitude;

            if (distanceRemaining <= distanceThisFrame)
            {
                Destroy(gameObject);
                return;
            }

            transform.Translate(_direction * distanceThisFrame, Space.World);
        }
    }
}

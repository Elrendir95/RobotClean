using System;
using UnityEngine;

namespace Components.Enemy
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 5f;

        private Vector3 _targetPosition;

        public void SetTarget(Vector3 target)
        {
            _targetPosition = target;
        }

        public void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, projectileSpeed * Time.deltaTime);

            if (transform.position == _targetPosition)
            {
                Destroy(gameObject);
            }
        }
    }
}
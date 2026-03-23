using System;
using Components.EventSystem;
using System.Collections;
using Components.StateMachine;
using Components.StateMachine.States;
using Library.References;
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

        [Header("Damage Progression")]
        [SerializeField] private FloatReference distance;
        [SerializeField, Tooltip("Damage values for each level")]
        private float[] damageSteps = { 12f, 16f, 20f, 25f, 30f };
        [SerializeField, Tooltip("Distance thresholds to reach each damage step")]
        private float[] distanceThresholds = { 0f, 500f, 1000f, 2000f, 4000f };

        [Header("Invincibility")]
        [SerializeField, Tooltip("Invincibility time after obstacle hits in seconds")]
        private float invincibilityTime = 1.5f;

        private bool _isInvincible = false;
        private bool _isActive = false;
        private int _currentDamageIndex;
        private float _obstacleDamage => damageSteps[_currentDamageIndex];

        private Vector3 PlayerSpherePosition => transform.position + sphereCenter;

        private void Awake()
        {
            Events.OnStateChanged += OnStateChanged;
            distance.OnValueChanged.AddListener(OnDistanceUpdate);
        }

        private void OnDistanceUpdate(float newDistance)
        {
            if (_currentDamageIndex + 1 >= distanceThresholds.Length) return;
            if (newDistance >= distanceThresholds[_currentDamageIndex + 1])
            {
                _currentDamageIndex++;
                Debug.Log($"New Damage = {_obstacleDamage}");
            }
        }

        private void OnDestroy()
        {
            Events.OnStateChanged -= OnStateChanged;
            distance.OnValueChanged.RemoveListener(OnDistanceUpdate);
        }

        private void OnStateChanged(State newState)
        {
            _isActive = newState is GameState;
            if (!_isActive) Events.OnPlayerInvincible?.Invoke(false);
        }

        private void Update()
        {
            // If we are invincible can't Collect, and can't hit an other obstacle
            if (_isInvincible || !_isActive) return;
            CheckObstacle();
            CheckCollectable();
        }

        private Collider[] _collectableHits = new Collider[4];

        private void CheckCollectable()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(PlayerSpherePosition, collectableSphereRadius, _collectableHits, collectableLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_collectableHits[i].TryGetComponent<Collectable>(out var collectable))
                {
                    collectable.OnCollect(gameObject);
                }
            }
        }

        private void CheckObstacle()
        {
            // If we are invincible do an early return
            if (_isInvincible) return;

            Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, obstacleSphereRadius, obstacleLayer);
            if (hitColliders.Length > 0)
            {
                Events.UpdateLife?.Invoke(-_obstacleDamage);
                StartCoroutine(InvincibilityCoroutine());
            }
        }

        IEnumerator InvincibilityCoroutine()
        {
            _isInvincible = true;
            Events.OnPlayerInvincible?.Invoke(_isInvincible);
            yield return new WaitForSeconds(invincibilityTime);
            _isInvincible = false;
            Events.OnPlayerInvincible?.Invoke(_isInvincible);
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

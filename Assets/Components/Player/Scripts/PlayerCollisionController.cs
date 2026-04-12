using System;
using Components.EventSystem;
using System.Collections;
using Components;
using Components.Collectible;
using Components.Skills;
using Components.StateMachine;
using Components.StateMachine.States;
using Library.References;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [Header("Running Settings")]
        [SerializeField] private Vector3 sphereCenter;
        [SerializeField, Tooltip("Center when sliding")] private Vector3 slidingCenter;
        [SerializeField, Tooltip("Obstacle sphere radius (red sphere)")] private float obstacleSphereRadius = 0.5f;
        [SerializeField, Tooltip("Collectable sphere radius(green sphere)")] private float collectableSphereRadius = 1f;
        [Header("Layers Settings")]
        [SerializeField] private LayerMask collectableLayer;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private LayerMask projectileLayer;

        [Header("Damage Progression")]
        [SerializeField] private FloatReference distance;
        [FormerlySerializedAs("damageSteps")] [SerializeField, Tooltip("Damage values for each level")]
        private float[] damageObstacleSteps = { 12f, 16f, 20f, 25f, 30f };
        [FormerlySerializedAs("distanceThresholds")] [SerializeField, Tooltip("Distance thresholds to reach each damage step")]
        private float[] distanceObstacleThresholds = { 0f, 500f, 1000f, 2000f, 4000f };
        [SerializeField, Tooltip("Projectile Damage values for each level")]
        private float[] damageProjectileSteps = { 10f, 15f, 20f };
        [SerializeField, Tooltip("Distance thresholds to reach each damage step")]
        private float[] distanceProjectileThresholds = { 0f, 2000f, 4000f };


        [Header("Invincibility")]
        [SerializeField, Tooltip("Invincibility time after obstacle hits in seconds")]
        private float invincibilityTime = 1.5f;

        private bool _isInvincible = false;
        private bool _isActive = false;
        private bool _isSliding = false;

        private int _currentObstacleDamageIndex;
        private float _obstacleDamage => damageObstacleSteps[_currentObstacleDamageIndex] * _armor.Value;
        private int _currentProjectileDamageIndex;
        private float _projectileDamage => damageProjectileSteps[_currentProjectileDamageIndex] * _armor.Value;

        private Vector3 PlayerSpherePosition => transform.position + (_isSliding ? slidingCenter : sphereCenter);

        private Skill _armor;

        private void Awake()
        {
            Events.OnStateChanged += OnStateChanged;
            Events.OnPlayerSlidingDown += OnPlayerSlidingDown;
            distance.OnValueChanged.AddListener(OnDistanceUpdate);
        }

        private void Start()
        {
            // Get the Armor Skill
            _armor = ScriptableObjectDatabase.Get<Skill>("ArmorSkill");
            Debug.Log($"Armor Bonus Value : {_armor.Value}");
        }

        private void OnDistanceUpdate(float newDistance)
        {
            if (_currentObstacleDamageIndex + 1 < distanceObstacleThresholds.Length)
            {
                if (newDistance >= distanceObstacleThresholds[_currentObstacleDamageIndex + 1])
                {
                    _currentObstacleDamageIndex++;
                }
            }

            if (_currentProjectileDamageIndex + 1 < damageProjectileSteps.Length)
            {
                if (newDistance >= distanceProjectileThresholds[_currentProjectileDamageIndex + 1])
                {
                    _currentProjectileDamageIndex++;
                }
            }
        }

        private void OnDestroy()
        {
            Events.OnStateChanged -= OnStateChanged;
            Events.OnPlayerSlidingDown -= OnPlayerSlidingDown;
            distance.OnValueChanged.RemoveListener(OnDistanceUpdate);
        }

        private void OnPlayerSlidingDown(bool sliding)
        {
            _isSliding = sliding;
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
            CheckProjectile();
        }

        private readonly Collider[] _collidersHits = new Collider[4];

        private void CheckProjectile()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(PlayerSpherePosition, collectableSphereRadius, _collidersHits, projectileLayer);
            if (hitCount > 0)
            {
                Events.UpdateLife?.Invoke(-_projectileDamage);
                Destroy(_collidersHits[0].gameObject);
                StartCoroutine(InvincibilityCoroutine());
            }
        }


        private void CheckCollectable()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(PlayerSpherePosition, collectableSphereRadius, _collidersHits, collectableLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_collidersHits[i].TryGetComponent<Collectable>(out var collectable))
                {
                    collectable.OnCollect(gameObject);
                }
                else
                {
                    Debug.LogError($"No Collectable Behaviour on  collectable {_collidersHits[i].name}");
                }
            }
        }

        private void CheckObstacle()
        {
            // If we are invincible do an early return
            if (_isInvincible) return;

            int hitCount = Physics.OverlapSphereNonAlloc(PlayerSpherePosition, obstacleSphereRadius, _collidersHits, obstacleLayer);
            if (hitCount > 0)
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
            Gizmos.DrawWireSphere(transform.position + sphereCenter, obstacleSphereRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + slidingCenter, obstacleSphereRadius);
            Gizmos.color = Color.lightGreen;
            Gizmos.DrawWireSphere(PlayerSpherePosition, collectableSphereRadius);
        }
#endif
    }
}

using Components.StateMachine;
using Components.StateMachine.States;
using UnityEngine;

namespace Components.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private ProjectileController projectilePrefab;
        [SerializeField] private Transform spawnProjectilePosition;
        [SerializeField] private float projectileDelay = 1f;
        
        [SerializeField] private Animator animator;

        private Transform _player;
        private bool _isActive;
        private float _timer;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;
            _isActive = true;
            EventSystem.Events.OnStateChanged += OnStateChanged;
        }

        private void OnStateChanged(State state)
        {
            _isActive = state is GameState;
        }

        private void ThrowProjectile()
        {
            ProjectileController projectile = Instantiate(projectilePrefab, spawnProjectilePosition.position, Quaternion.identity);
            projectile.SetTarget(_player.position);
        }

        private void Update()
        {
            if (!_isActive) return;

            _timer += Time.deltaTime;
            if (_timer >= projectileDelay)
            {
                _timer = 0f;
                ThrowProjectile();
            }
        }
    }
}
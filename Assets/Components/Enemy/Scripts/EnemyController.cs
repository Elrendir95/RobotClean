using Components.StateMachine;
using Components.StateMachine.States;
using TreeEditor;
using UnityEngine;

namespace Components.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private ProjectileController projectilePrefab;
        [SerializeField] private Transform spawnProjectilePosition;
        [SerializeField] private float projectileDelay = 1f;
        [SerializeField] private float attackRange = 70f;
        [SerializeField] private float playerSafeZone = 5f;
        [SerializeField] private Animator animator;

        private Transform _player;
        private bool _isActive;
        private float _timer;

        private static readonly int Throw = Animator.StringToHash("Throw");

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
            animator.SetTrigger(Throw);
            ProjectileController projectile = Instantiate(projectilePrefab, spawnProjectilePosition.position, Quaternion.identity);
            projectile.SetTarget(_player.position);
            projectile.transform.SetParent(null, true);
        }

        private void Update()
        {
            transform.LookAt(_player.position);
            if (transform.position.z <= _player.position.z + playerSafeZone) _isActive = false;
            if (!_isActive) return;

            if (Vector3.Distance(transform.position, _player.position) > attackRange) return;

            _timer += Time.deltaTime;
            if (_timer >= projectileDelay)
            {
                _timer = 0f;
                ThrowProjectile();
            }
        }
    }
}

using Components.EventSystem;
using Library.References;
using UnityEngine;

namespace Components.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy parameters")]
        [SerializeField] private EnemyController[] enemies;

        [Header("Distance parameters")]
        [SerializeField] private FloatReference currentDistance;
        [SerializeField] private float[] distancesTriggers;
        [SerializeField] private float[] distancesIntervals;

        private int _distanceSpawnIndex;
        private float _lastDistanceUpdate;
        private float _currentDistance;

        private void Start()
        {
            currentDistance.OnValueChanged.AddListener(OnDistanceUpdated);
            Events.OnChunkSpawned += OnChunkSpawned;
        }


        private void OnDestroy()
        {
            currentDistance.OnValueChanged.RemoveListener(OnDistanceUpdated);
            Events.OnChunkSpawned -= OnChunkSpawned;
        }

        private void CheckDistance()
        {
            if (_distanceSpawnIndex + 1 >= distancesIntervals.Length) return;
            if (currentDistance.Value > distancesTriggers[_distanceSpawnIndex + 1]) _distanceSpawnIndex++;
        }

        private bool IsSpawnDistanceReached(float chunkDistance) => _distanceSpawnIndex < distancesIntervals.Length && _currentDistance + chunkDistance > distancesIntervals[_distanceSpawnIndex];

        private void OnDistanceUpdated(float distance)
        {
            CheckDistance();

            _currentDistance += distance - _lastDistanceUpdate;
            _lastDistanceUpdate = distance;
        }

        private void OnChunkSpawned(ChunkController chunk)
        {
            if (chunk.TryGetComponent<EnemyChunkController>(out var component))
            {
                if (!IsSpawnDistanceReached(chunk.transform.position.z)) return;
                SpawnEnemyOnChunk(component);
                _currentDistance = 0;
            }
        }

        private void SpawnEnemyOnChunk(EnemyChunkController chunk)
        {
            Transform spawnPoint = chunk.EnemySpawnPoints[Random.Range(0, chunk.EnemySpawnPoints.Length)];

            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoint);
        }
    }
}

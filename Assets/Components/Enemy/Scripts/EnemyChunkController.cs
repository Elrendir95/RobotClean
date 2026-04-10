using UnityEngine;

namespace Components.Enemy
{
    public class EnemyChunkController : MonoBehaviour
    {
        // Available Spawn points for enemies on the chunk
        [SerializeField] private Transform[] spawnPoints;

        public Transform[] EnemySpawnPoints => spawnPoints;
    }
}

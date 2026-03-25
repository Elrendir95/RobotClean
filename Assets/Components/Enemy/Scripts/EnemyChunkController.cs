using UnityEngine;

namespace Components.Enemy
{
    public class EnemyChunkController : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;

        public Transform[] EnemySpawnPoints => spawnPoints;
    }
}

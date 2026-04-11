using UnityEngine;
public class ObstacleController : ChunkController
{
    [SerializeField] private Transform safePath;
    [SerializeField] private Transform electronicsSpawn;
    public Vector3 SafePosition => safePath.position;
    public Vector3 ElectronicsSpawnPosition => electronicsSpawn.position;
}

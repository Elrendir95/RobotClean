using UnityEngine;
public class ObstacleController : ChunkController
{
    [SerializeField] private Transform safePath;
    public Vector3 SafePosition => safePath.position;
}

using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private Transform endAnchor;
    public Vector3 EndAnchor => endAnchor.position;

    public bool IsBehindPlayer => EndAnchor.z <= 0;
}

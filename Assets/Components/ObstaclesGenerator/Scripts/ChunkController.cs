using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private Transform endAnchor;
    public Vector3 EndAnchor => endAnchor.position;
}

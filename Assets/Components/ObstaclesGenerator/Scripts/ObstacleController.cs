using System.Collections.Generic;
using Library.References;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("Translation speed of chunks in m/s")] private FloatReference translationSpeed;
    [SerializeField] private IntReference activeChunkCount;
    [Header("Components")]
    [SerializeField] private ChunkController[] chunksPool;

    private List<ChunkController> instatiatedChunks = new();

    private void Start()
    {
        AddBaseChunk();
    }

    private void AddBaseChunk()
    {
        for (int i = 0; i < activeChunkCount.Value; i++)
        {
            ChunkController chunk;
            if (i == 0)
            {
                chunk = AddChunk(transform.position);
                instatiatedChunks.Add(chunk);
                continue;
            }
            chunk = AddChunk(LastChunk.EndAnchor);
            instatiatedChunks.Add(chunk);
        }
    }

    private ChunkController AddChunk(Vector3 position)
    {
        if (chunksPool == null || chunksPool.Length == 0)
        {
            Debug.LogError($"{nameof(chunksPool)} is null or empty");
            return null;
        }
        var index = Random.Range(0, chunksPool.Length);
        ChunkController chunk = Instantiate(chunksPool[index], position, Quaternion.identity);
        return chunk;
    }

    private ChunkController LastChunk => instatiatedChunks[^1];
}

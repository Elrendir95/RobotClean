using System.Collections.Generic;
using Library.References;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("Translation speed of chunks in m/s")] private FloatReference translationSpeed;
    [SerializeField] private IntReference activeChunkCount;
    [SerializeField] private IntReference behindChunkCount;

    [Header("Components")]
    [SerializeField] private ChunkController[] chunksPool;

    private readonly List<ChunkController> _instantiatedChunks = new();

    private void Start()
    {
        AddBaseChunk();
    }

    private void Update()
    {
        foreach (ChunkController chunk in _instantiatedChunks)
        {
            chunk.transform.Translate(translationSpeed.Value * Time.deltaTime * Vector3.back);
        }

        UpdateChunks();
    }

    private void UpdateChunks()
    {
        List<ChunkController> behindChunks = new List<ChunkController>();
        foreach (ChunkController chunk in _instantiatedChunks)
        {
            if (chunk.IsBehindPlayer)
            {
                behindChunks.Add(chunk);
            }
        }

        // Delete chunks behind player
        if (behindChunks.Count > behindChunkCount.Value)
        {
            int chunkToDeleteCount = behindChunks.Count - behindChunkCount.Value;
            for (int i = 0; i < chunkToDeleteCount; i++)
            {
                ChunkController chunkToDelete = behindChunks[i];
                _instantiatedChunks.Remove(chunkToDelete);
                Destroy(chunkToDelete.gameObject);
            }
        }

        int missingChunkCount = activeChunkCount.Value - _instantiatedChunks.Count;
        for (int i = 0; i < missingChunkCount; i++)
        {
            var chunk = AddChunk(LastChunk.EndAnchor);
            _instantiatedChunks.Add(chunk);
        }
    }

    private void AddBaseChunk()
    {
        for (int i = 0; i < activeChunkCount.Value; i++)
        {
            ChunkController chunk;
            if (i == 0)
            {
                chunk = AddChunk(transform.position);
                _instantiatedChunks.Add(chunk);
                continue;
            }
            chunk = AddChunk(LastChunk.EndAnchor);
            _instantiatedChunks.Add(chunk);
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

    private ChunkController LastChunk => _instantiatedChunks[^1];
}

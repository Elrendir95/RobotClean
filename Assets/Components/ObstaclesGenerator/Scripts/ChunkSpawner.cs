using System.Collections.Generic;
using Components.EventSystem;
using Library.References;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("Translation speed of chunks in m/s")] private FloatReference translationSpeed;
    [SerializeField] private IntReference activeChunkCount;
    [SerializeField] private IntReference behindChunkCount;
    [SerializeField] protected float distanceToNextChunk;

    [Header("Components")]
    [SerializeField] private ChunkController[] chunksPool;

    private readonly List<ChunkController> _instantiatedChunks = new();

    private void Start()
    {
        AddBaseChunk();
    }

    protected virtual void Update()
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
            var chunk = AddChunk(LastChunk.EndAnchor + new Vector3(0f, 0f, distanceToNextChunk));
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
                chunk = AddChunk(transform.position + new Vector3(0f, 0f, distanceToNextChunk));
                _instantiatedChunks.Add(chunk);
                continue;
            }
            chunk = AddChunk(LastChunk.EndAnchor + new Vector3(0f, 0f, distanceToNextChunk));
            _instantiatedChunks.Add(chunk);
        }
    }

    protected virtual ChunkController AddChunk(Vector3 position)
    {
        if (chunksPool == null || chunksPool.Length == 0)
        {
            Debug.LogError($"{nameof(chunksPool)} is null or empty");
            return null;
        }
        var index = Random.Range(0, chunksPool.Length);
        ChunkController chunk = Instantiate(chunksPool[index], position, Quaternion.identity);
        Events.OnChunkSpawned?.Invoke(chunk);
        return chunk;
    }

    private ChunkController LastChunk => _instantiatedChunks[^1];
}

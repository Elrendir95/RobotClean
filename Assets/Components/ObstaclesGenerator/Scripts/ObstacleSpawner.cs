using Components.Collectible;
using Components.EventSystem;
using Components.StateMachine;
using Components.StateMachine.States;
using UnityEngine;

public class ObstacleSpawner : ChunkSpawner
{
    [Header("Obstacle Settings")]
    [SerializeField] private float[] obstacleDistances;
    [SerializeField, Tooltip("Times in seconds")] private float[] timeToNextDensity;
    [Header("Collectables Settings")]
    [SerializeField] private Collectable[] collectablesToSpawn;
    [SerializeField, Tooltip("Distance between obstacles in meters")] private float collectableDistance;

    private GameState _gameState;
    private bool _isInGameState;
    private int _currentObstacleIndex;
    private ObstacleController _lastObstacle;
    private float _lastCollectibleDistance;

    private void Awake()
    {
        Events.OnStateChanged += OnStateChanged;
        _lastCollectibleDistance = collectableDistance;
        if (collectablesToSpawn.Length == 0) collectableDistance = 0f;
    }

    private void OnDestroy()
    {
        Events.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(State newState)
    {
        if (newState is not GameState gameState)
        {
            _isInGameState = false;
            return;
        }
        _gameState = gameState;
        _isInGameState = true;
    }

    protected override void Update()
    {
        base.Update();

        if (!_isInGameState || _currentObstacleIndex >= timeToNextDensity.Length) return;

        if (_gameState.Timer > timeToNextDensity[_currentObstacleIndex])
        {
            distanceToNextChunk = obstacleDistances[_currentObstacleIndex];
            _currentObstacleIndex++;
        }
    }

    /// <summary>
    /// Override parent AddChunk to add Collectible Logics
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    protected override ChunkController AddChunk(Vector3 position)
    {
        var newChunk = base.AddChunk(position);
        if (newChunk is not ObstacleController obstacleChunk)
        {
            Debug.LogError("Not obstacle chunk, handled by ObstacleSpawner");
            return newChunk;
        }

        if (collectableDistance <= 0f) return newChunk;

        SpawnCollectible(obstacleChunk);
        return obstacleChunk;
    }

    /// <summary>
    /// Spawn random collectible between the previous obstacle and the new one
    /// </summary>
    /// <param name="obstacleChunk"></param>
    private void SpawnCollectible(ObstacleController obstacleChunk)
    {
        Vector3 destinationSafe = obstacleChunk.SafePosition;
        Vector3 lastSafePosition = _lastObstacle ? _lastObstacle.SafePosition : Vector3.zero;
        Vector3 direction = (destinationSafe - lastSafePosition).normalized;

        float toEnd = Vector3.Distance(lastSafePosition, destinationSafe);
        float currentDistance = _lastCollectibleDistance;

        while (currentDistance < toEnd)
        {
            Vector3 spawnPosition = lastSafePosition + direction * currentDistance;
            var collectable = Instantiate(collectablesToSpawn[Random.Range(0, collectablesToSpawn.Length)], spawnPosition, Quaternion.identity);
            // Attach the collectible to the chunk, to follow chunk mouvement
            collectable.transform.SetParent(obstacleChunk.transform, true);
            currentDistance += collectableDistance;
        }
        _lastCollectibleDistance = collectableDistance - (toEnd - (currentDistance - collectableDistance));
        _lastObstacle = obstacleChunk;
    }
}

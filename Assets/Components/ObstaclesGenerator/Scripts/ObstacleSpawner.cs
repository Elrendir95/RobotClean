using System;
using Components.EventSystem;
using Components.StateMachine;
using Components.StateMachine.States;
using Library.References;
using UnityEngine;

public class ObstacleSpawner : ChunkSpawner
{
    [SerializeField] private float[] obstacleDistances;
    [SerializeField, Tooltip("Times in seconds")] private float[] timeToNextDensity;

    private GameState _gameState;
    private bool _isInGameState;
    private int _currentObstacleIndex;

    private void Awake()
    {
        Events.OnStateChanged += OnStateChanged;
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
}

using Components.AudioSystem;
using Components.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.StateMachine.States
{
    public class GameState : State
    {
        public float Timer { get; private set;  }

        public GameState(StateMachine stateMachine, float lastTimer = 0) : base(stateMachine) {Timer = lastTimer;}

        public override void Enter()
        {
            Events.OnLifeCountChanged += OnLifeCountChanged;
            Events.PlayAudio?.Invoke(ScriptableObjectDatabase.Get<AudioSO>("GameMusicLoop"));
        }

        private void OnLifeCountChanged(float lifeCount)
        {
            if (lifeCount <= 0)
            {
                StateMachine.ChangeState(new GameOverState(StateMachine));
            }
        }

        public override void Update()
        {
            Timer += Time.deltaTime;
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                StateMachine.ChangeState(new PauseState(StateMachine, Timer));
            }
        }

        public override void Exit()
        {
            Events.OnLifeCountChanged -= OnLifeCountChanged;
            Events.StopAllLoops?.Invoke();
        }
    }
}

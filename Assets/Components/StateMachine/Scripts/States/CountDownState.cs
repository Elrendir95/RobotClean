using Components.AudioSystem;
using UnityEngine;
using Components.EventSystem;

namespace Components.StateMachine.States
{
    public class CountDownState : State
    {
        private float _initialTime = 3f;

        public float Timer { get; private set; } = 0f;
        private int _intTimer = 0;

        public CountDownState(StateMachine stateMachine) : base(stateMachine) {}

        private AudioSO _tickSFX;
        private AudioSO _goSFX;

        public override void Enter()
        {
            _tickSFX = ScriptableObjectDatabase.Get<AudioSO>("countdownTickSFX");
            _goSFX =  ScriptableObjectDatabase.Get<AudioSO>("countdownEndSFX");

            PlayTickSFX();

            Timer = _initialTime;
            _intTimer = Mathf.FloorToInt(_initialTime);
        }

        public override void Update()
        {
            Timer -= Time.deltaTime;

            if (Timer > 0f)
            {
                var newIntTimer = Mathf.FloorToInt(Timer);
                if (_intTimer != newIntTimer)
                {
                    PlayTickSFX();
                    _intTimer = newIntTimer;
                }
                return;
            }

            if (_goSFX != null)
            {
                Events.PlayAudio?.Invoke(_goSFX);
            }

            // Countdown finished go to GameState
            var gameState = new GameState(StateMachine);
            StateMachine.ChangeState(gameState);
        }

        public override void Exit()  {}

        private void PlayTickSFX()
        {
            if (_tickSFX != null)
            {
                Events.PlayAudio?.Invoke(_tickSFX);
            }
        }
    }
}

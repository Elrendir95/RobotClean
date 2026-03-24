using UnityEngine.InputSystem;

namespace Components.StateMachine.States
{
    public class PauseState : State
    {
        private float _gameTimer;
        public PauseState(StateMachine stateMachine, float timer) : base(stateMachine)
        {
            _gameTimer = timer;
        }

        private void ResumeGame()
        {
            StateMachine.ChangeState(new GameState(StateMachine, _gameTimer));
        }

        public override void Enter()
        {
            EventSystem.Events.OnResumeGame += ResumeGame;
        }

        public override void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ResumeGame();
            }
        }

        public override void Exit()
        {
            EventSystem.Events.OnResumeGame -= ResumeGame;
        }
    }
}

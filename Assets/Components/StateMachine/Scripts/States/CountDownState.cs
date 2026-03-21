using UnityEngine;

namespace Components.StateMachine.States
{
    public class CountDownState : State
    {
        private float _initialTime = 3f;

        public float Timer { get; private set; } = 0f;

        public CountDownState(StateMachine stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            Debug.Log("CountDownState:Enter");
            Timer = _initialTime;
        }

        public override void Update()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0f)
            {

                return;
            }
            Debug.Log("CountDownState:Update:Finished");
            // Countdown finished go to GameState
            var gameState = new GameState(StateMachine);
            StateMachine.ChangeState(gameState);
        }

        public override void Exit()
        {
            Debug.Log("CountDownState:Exit");
        }
    }
}

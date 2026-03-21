using Components.EventSystem;
using UnityEngine;

namespace Components.StateMachine.States
{
    public class GameState : State
    {
        public float Timer { get; private set;  }

        public GameState(StateMachine stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            Events.OnLifeCountChanged += OnLifeCountChanged;
            Timer = 0f;
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
        }

        public override void Exit()
        {
            Events.OnLifeCountChanged -= OnLifeCountChanged;
        }
    }
}

using Components.EventSystem;

namespace Components.StateMachine.States
{
    public class GameState : State
    {
        public GameState(StateMachine stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            Events.OnLifeCountChanged += OnLifeCountChanged;
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
        }

        public override void Exit()
        {
            Events.OnLifeCountChanged -= OnLifeCountChanged;
        }
    }
}

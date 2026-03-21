using System;
using Components.StateMachine.States;
using UnityEngine;

namespace Components.StateMachine
{
    public class StateController : MonoBehaviour
    {
        private StateMachine _stateMachine;

        public void Start()
        {
            _stateMachine = new StateMachine();
            var initialState = new CountDownState(_stateMachine);
            _stateMachine.ChangeState(initialState);
        }

        public void Update()
        {
            _stateMachine.Update();
        }
    }
}

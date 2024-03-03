using System;
using Entities.MobAI.States;
using General;
using UnityEngine;

namespace Entities.MobAI
{
    public class TestMobController : EntityController
    {

        private StateMachine _stateMachine;
        
        private void Start()
        {
            IdleState idleState = new IdleState(this, 2);
            RunToRandomPointState runToRandomPointState = new RunToRandomPointState(this, 1f, 3f);
            RunToFixedPointState runToStartState = new RunToFixedPointState(this, Vector2.zero);
            
            _stateMachine = new StateMachine.Builder(idleState)
                .AddOnFinishedTransition(idleState, runToRandomPointState)
                .AddOnFinishedTransition(runToRandomPointState, idleState)
                .AddAnyTransition(runToStartState, () => Input.GetKeyDown(KeyCode.R))
                .AddOnFinishedTransition(runToStartState, idleState)
                .Build();
        }

        private void Update() => _stateMachine.Tick(Time.deltaTime);

        protected override void FixedUpdateAddOn()
        {
            
        }
    }
}
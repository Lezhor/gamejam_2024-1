using General;
using UnityEngine;

namespace Entities.MobAI.States
{
    public class IdleState : StateMachine.IState
    {
        public float TimeIdle { get; private set; }

        private readonly float _duration;

        private readonly EntityController _controller;

        public IdleState(EntityController controller) : this(controller, float.PositiveInfinity)
        {
        }
        
        public IdleState(EntityController controller, float duration)
        {
            _controller = controller;
            _duration = duration;
        }
        
        public void OnEnter()
        {
            TimeIdle = 0f;
            _controller.MoveVector = Vector2.zero;
        }

        public void Tick(float deltaTime)
        {
            TimeIdle += deltaTime;
        }

        public bool StateFinished()
        {
            return TimeIdle >= _duration;
        }

        public void OnExit()
        {
        }
    }
}
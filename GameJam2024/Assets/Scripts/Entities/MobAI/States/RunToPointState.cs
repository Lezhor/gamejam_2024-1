using General;
using UnityEngine;

namespace Entities.MobAI.States
{
    public abstract class RunToPointState : StateMachine.IState
    {
        protected readonly EntityController Controller;

        public delegate Vector3 DestinationPointFactory();

        private Vector3 _destinationPoint;

        public RunToPointState(EntityController controller)
        {
            Controller = controller;
        }
        
        public void OnEnter()
        {
            _destinationPoint = GenerateDestinationPoint();
        }

        public abstract Vector3 GenerateDestinationPoint();

        public void Tick(float deltaTime)
        {
            Controller.MoveVector = (_destinationPoint - Controller.transform.position).normalized;
        }

        public bool StateFinished()
        {
            return (Controller.transform.position - _destinationPoint).magnitude < 0.1f;
        }

        public void OnExit()
        {
            Controller.MoveVector = Vector2.zero;
        }
    }
}
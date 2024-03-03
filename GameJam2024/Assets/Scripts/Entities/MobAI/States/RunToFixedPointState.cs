using UnityEngine;

namespace Entities.MobAI.States
{
    public class RunToFixedPointState : RunToPointState
    {
        private readonly Vector2 _destination;
        
        public RunToFixedPointState(EntityController controller, Vector2 destination) : base(controller)
        {
            _destination = destination;
        }

        public override Vector3 GenerateDestinationPoint()
        {
            return _destination;
        }
    }
}
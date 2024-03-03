using UnityEngine;

namespace Entities.MobAI.States
{
    public class RunToRandomPointState : RunToPointState
    {
        private readonly float _minDistance;
        private readonly float _maxDistance;
        
        public RunToRandomPointState(EntityController controller, float minDistance, float maxDistance) : base(controller)
        {
            _minDistance = minDistance;
            _maxDistance = maxDistance;
        }

        public override Vector3 GenerateDestinationPoint()
        {
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            Vector3 randomDirection = new
            (
                Mathf.Cos(randomAngle),
                Mathf.Sin(randomAngle)
            );
            randomDirection *= Random.Range(_minDistance, _maxDistance);
            return Controller.transform.position + randomDirection;
        }
    }
}
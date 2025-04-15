using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

	public class DroneDetectorCT : ConditionTask 
    {
        public BBParameter<float> detectionRange = 5f;
        public BBParameter<Transform> playerTransform;

        protected override bool OnCheck()
        {
            Vector3 directionToPlayer = playerTransform.value.position - agent.transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer > detectionRange.value)
            {
                return false;
            }

            bool hasObstacle = Physics.Raycast(agent.transform.position, directionToPlayer.normalized, out RaycastHit hit, detectionRange.value);

            bool canSeePlayer = !hasObstacle || hit.transform == playerTransform.value;

            return true;
        }
    }
}
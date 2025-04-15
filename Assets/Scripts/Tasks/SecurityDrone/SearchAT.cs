using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class SearchAT : ActionTask {

        public BBParameter<Transform[]> waypoints;
        public BBParameter<float> moveSpeed = 3f;
        public BBParameter<float> stoppingDistance = 0.5f;
        public BBParameter<int> currentWaypoint;
        public BBParameter<bool> isMovingForward = true;

        public Transform droneTransform;
        private Vector3 currentTargetPos;

        protected override string OnInit()
        {
            currentWaypoint.value = Mathf.Clamp(currentWaypoint.value, 0, waypoints.value.Length - 1);
            currentTargetPos = waypoints.value[currentWaypoint.value].position;

            return null;
        }

        protected override void OnUpdate()
        {
            if (waypoints.value.Length == 0)
            {
                EndAction(false);
                return;
            }

            //Move
            droneTransform.position = Vector3.MoveTowards(
                droneTransform.position,
                currentTargetPos,
                moveSpeed.value * Time.deltaTime
            );

            //Rotate
            Vector3 direction = (currentTargetPos - droneTransform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                droneTransform.rotation = Quaternion.Slerp(
                    droneTransform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }

            if (Vector3.Distance(droneTransform.position, currentTargetPos) <= stoppingDistance.value)
            {
                UpdateWaypoint();
                currentTargetPos = waypoints.value[currentWaypoint.value].position;
                EndAction(true);
            }
        }

        void UpdateWaypoint()
        {
            currentWaypoint.value--;
            if (currentWaypoint.value < 0)
                currentWaypoint.value = waypoints.value.Length - 1;
        }
    }
}
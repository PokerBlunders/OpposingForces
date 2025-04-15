using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.AI;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions 
{

	public class PatrolAT : ActionTask {

        public BBParameter<Transform[]> waypoints;
        public BBParameter<float> moveSpeed = 3f;
        public BBParameter<float> stoppingDistance = 0.5f;

        public BBParameter<int> currentWaypoint;
        private bool isMoving = true;

        private Transform guardTransform;
        public BBParameter<NavMeshAgent> navAgent;

        protected override string OnInit()
        {
            guardTransform = agent.transform;

            if (currentWaypoint.value >= waypoints.value.Length)
                currentWaypoint.value = 0;

            navAgent.value.speed = moveSpeed.value;
            return null;
        }

        protected override void OnUpdate()
        {
            Transform target = waypoints.value[currentWaypoint.value];

            navAgent.value.SetDestination(target.position);

            if (Vector3.Distance(guardTransform.position, target.position) <= stoppingDistance.value)
            {
                UpdateWaypoint();
                EndAction(true);
            }
        }

        void UpdateWaypoint()
        {
            if (isMoving)
            {
                currentWaypoint.value = (currentWaypoint.value + 1) % waypoints.value.Length;
            }
            else
            {
                currentWaypoint.value--;
                if (currentWaypoint.value < 0)
                    currentWaypoint.value = waypoints.value.Length - 1;
            }
        }
    }
}
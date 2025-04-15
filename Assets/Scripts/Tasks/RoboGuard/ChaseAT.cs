using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace NodeCanvas.Tasks.Actions 
{
	public class ChaseAT : ActionTask 
    {
        public BBParameter<float> chaseSpeed = 5f;
        public BBParameter<float> maxLostTime = 5f;
        public BBParameter<float> captureDistance = 1.5f;
        public BBParameter<float> detectionRange = 10f;
        public BBParameter<float> fovAngle = 90f;
        public BBParameter<LayerMask> obstacleLayers;

        private float originalSpeed;
        public BBParameter<Transform> target;
        public BBParameter<NavMeshAgent> navAgent;
        private Transform guardTransform;
        private float timeSinceLastSeen;
        public BBParameter<bool> playerDetected;
        public BBParameter<Image> progressBar;

        protected override string OnInit()
        {
            guardTransform = agent.transform;
            originalSpeed = navAgent.value.speed;
            return null;
        }

        protected override void OnExecute()
        {
            timeSinceLastSeen = 0f;
            navAgent.value.isStopped = false;
            navAgent.value.speed = chaseSpeed.value;
            navAgent.value.SetDestination(target.value.position);
        }

        protected override void OnUpdate()
        {
            float distance = Vector3.Distance(guardTransform.position, target.value.position);
            if (distance <= captureDistance.value)
            {
                Debug.Log("Player captured!");
                playerDetected.value = false;
                ResetGame();
                return;
            }

            navAgent.value.SetDestination(target.value.position);

            bool canSeePlayer = CheckPlayerVisibility();

            if (canSeePlayer)
            {
                timeSinceLastSeen = 0f;
            }
            else
            {
                timeSinceLastSeen += Time.deltaTime;

                if (timeSinceLastSeen >= maxLostTime.value)
                {
                    Debug.Log("Lost player!");
                    navAgent.value.speed = originalSpeed; 
                    playerDetected.value = false;
                    progressBar.value.fillAmount = 0;
                    EndAction(false);
                }
            }
        }

        bool CheckPlayerVisibility()
        {
            Vector3 directionToTarget = target.value.position - guardTransform.position;
            float distance = directionToTarget.magnitude;

            if (distance > detectionRange.value)
                return false;

            if (Vector3.Angle(guardTransform.forward, directionToTarget.normalized) > fovAngle.value / 2)
                return false;

            if (Physics.Raycast(guardTransform.position, directionToTarget.normalized, out RaycastHit hit, detectionRange.value, obstacleLayers.value))
            {
                return hit.transform == target.value;
            }

            return true;
        }

        void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        protected override void OnStop()
        {
            navAgent.value.ResetPath();
            navAgent.value.velocity = Vector3.zero;
        }
    }
}
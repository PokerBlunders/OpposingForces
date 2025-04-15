using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;


namespace NodeCanvas.Tasks.Actions 
{

	public class GuardScanAT : ActionTask 
    {
        public BBParameter<float> detectionSpeed = 1f;
        public BBParameter<float> cooldownSpeed = 2f;
        public BBParameter<float> maxDetection = 3f;
        public BBParameter<float> detectionRange = 10f;
        public BBParameter<Transform> playerTransform;
        public BBParameter<LayerMask> obstacleLayers;
        public BBParameter<Image> progressBar;
        public BBParameter<bool> playerDetected;

        public BBParameter<NavMeshAgent> navAgent;
        private float currentDetection;

        protected override void OnExecute()
        {
            navAgent.value.isStopped = true;
            navAgent.value.velocity = Vector3.zero;
            currentDetection = 0;
            UpdateProgressUI();
        }

        protected override void OnUpdate()
        {
            CheckPlayerVisibility();

            if (CheckPlayerVisibility())
            {
                currentDetection += detectionSpeed.value * Time.deltaTime;
                if (currentDetection >= maxDetection.value)
                {
                    Debug.Log("Full detection!");
                    playerDetected.value = true;
                    EndAction(true);
                }
            }
            else
            {
                currentDetection = Mathf.Max(0, currentDetection - cooldownSpeed.value * Time.deltaTime);
                if (currentDetection <= 0)
                {
                    Debug.Log("Lost player");
                    EndAction(false);
                }
            }

            UpdateProgressUI();
        }

        bool CheckPlayerVisibility()
        {
            Vector3 direction = playerTransform.value.position - agent.transform.position;
            float distance = direction.magnitude;

            if (distance > detectionRange.value)
                return false;

            if (Physics.Raycast(agent.transform.position, direction.normalized, out RaycastHit hit, detectionRange.value, obstacleLayers.value))
            {
                return hit.transform == playerTransform.value;
            }
            return true;
        }

        void UpdateProgressUI()
        {
            progressBar.value.fillAmount = currentDetection / maxDetection.value;
        }

        protected override void OnStop()
        {
            navAgent.value.isStopped = false;
            progressBar.value.fillAmount = 0;
        }
    }
}
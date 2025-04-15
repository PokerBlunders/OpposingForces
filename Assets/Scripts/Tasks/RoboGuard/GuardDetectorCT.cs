using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace NodeCanvas.Tasks.Conditions
{

    public class GuardDetectorCT : ConditionTask
    {
        public BBParameter<float> detectionRange = 10f;
        public BBParameter<float> fovAngle = 90f;
        public BBParameter<LayerMask> obstacleLayers;
        public BBParameter<Transform> playerTransform;
        public BBParameter<bool> playerSpotted;
        public BBParameter<bool> playerDetected;
        public BBParameter<Image> progressBar;

        protected override bool OnCheck()
        {
            Vector3 directionToPlayer = playerTransform.value.position - agent.transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;


            if (playerDetected.value == true)
            {
                progressBar.value.fillAmount = 1;
                return true;
            }

            if (distanceToPlayer > detectionRange.value)
            {
                playerSpotted.value = false;
                return false;
            }

            if (Vector3.Angle(agent.transform.forward, directionToPlayer.normalized) > fovAngle.value / 2)
            {
                playerSpotted.value = false;
                return false;
            }

            if (Physics.Raycast(agent.transform.position, directionToPlayer.normalized, out RaycastHit hit, detectionRange.value, obstacleLayers.value))
            {
                playerSpotted.value = hit.transform == playerTransform.value;
                return playerSpotted.value;
            }

            playerSpotted.value = true;
            return false;
        }

    }
}
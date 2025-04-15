using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionDrone : MonoBehaviour
{
    public float detectionRange = 5f;
    public int rayCount = 50;

    private LineRenderer lineRenderer;
    private Transform droneTransform;
    
    public Material normalMaterial;
    public Material alertMaterial;

    public GlobalBlackboard globalBlackboard;

    void Start()
    {
        droneTransform = transform;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = rayCount + 1;
        UpdateCircle();
    }

    void Update()
    {
        bool detected = globalBlackboard.GetVariable<bool>("playerDetected").value;

        UpdateCircle();

        if (detected == true)
        {
            lineRenderer.material = alertMaterial;
        }
        else
        {
            lineRenderer.material = normalMaterial;
        }
    }

    void UpdateCircle()
    {
        float angle = 0f;
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * detectionRange;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * detectionRange;

            lineRenderer.SetPosition(i, new Vector3(x, 0.1f, z));
            angle += angleStep;
        }
    }
    public bool IsPlayerInRadius(Transform player)
    {
        float distance = Vector3.Distance(droneTransform.position, player.position);
        return distance <= detectionRange;
    }
}

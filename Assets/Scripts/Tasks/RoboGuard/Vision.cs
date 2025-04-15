using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public float detectionRange = 10f;
    public float fovAngle = 90f;
    public int rayCount = 50;

    private LineRenderer lineRenderer;
    private Transform guardTransform;

    public Material normalMaterial;
    public Material alertMaterial;

    public GlobalBlackboard globalBlackboard;

    void Start()
    {
        guardTransform = transform;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = rayCount + 2;
    }

    void Update()
    {
        bool detected = globalBlackboard.GetVariable<bool>("playerDetected").value;

        DrawVisionCone();

        if (detected == true)
        {
            lineRenderer.material = alertMaterial;
        }
        else
        {
            lineRenderer.material = normalMaterial;
        }
    }

    void DrawVisionCone()
    {
        Vector3[] points = new Vector3[rayCount + 2];
        float angleStep = fovAngle / rayCount;
        float currentAngle = -fovAngle / 2;

        points[0] = guardTransform.position;

        for (int i = 1; i <= rayCount; i++)
        {
            Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * guardTransform.forward;
            points[i] = guardTransform.position + dir * detectionRange;
            currentAngle += angleStep;
        }

        points[rayCount + 1] = guardTransform.position;

        lineRenderer.SetPositions(points);
    }
}

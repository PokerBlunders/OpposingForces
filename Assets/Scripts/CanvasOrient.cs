using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOrient : MonoBehaviour
{
    public Camera targetCamera;
    public float rotationSpeed = 10f;

    void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 directionToCamera = targetCamera.transform.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}

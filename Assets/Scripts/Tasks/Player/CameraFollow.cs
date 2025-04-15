using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float positionSmooth = 5f;

    public bool followRotation = true;
    public float tiltAngle = 10f;
    public float rotationSmooth = 3f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmooth * Time.deltaTime
        );

        if (followRotation)
        {
            Quaternion baseRotation = Quaternion.LookRotation(target.forward, Vector3.up);
            Quaternion tiltRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
            Quaternion desiredRotation = baseRotation * tiltRotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmooth * Time.deltaTime);
        }
    }
}

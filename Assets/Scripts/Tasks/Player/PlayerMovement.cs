using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 180f;

    private Rigidbody rb;
    private Vector3 movement;
    private float rotateInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        rotateInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateInput -= 1f;
        if (Input.GetKey(KeyCode.E)) rotateInput += 1f;
    }

    void FixedUpdate()
    {
        if (rotateInput != 0f)
        {
            float rotationAmount = rotateInput * rotationSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        Vector3 moveDirection = (transform.forward * movement.z + transform.right * movement.x).normalized;
        rb.velocity = moveDirection * speed;
    }
}

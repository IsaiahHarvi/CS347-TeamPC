using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintSpeed = 8.0f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 5.0f;
    public float mouseSensitivity = 500f;
    public Camera playerCamera;
    public float adsFOV = 60f;
    public float normalFOV = 90f; 
    public float health = 100f;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool isSprinting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MovePlayer();
        MouseLook();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrouch();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        if (Input.GetMouseButtonDown(1)) //right mouse button
        {
            playerCamera.fieldOfView = adsFOV;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            playerCamera.fieldOfView = normalFOV;
        }

        if (health <= 0)
        {
            // Handle player death
        }
    }

    void FixedUpdate()
    {
        CheckGroundStatus();
    }

    void CheckGroundStatus()
    {
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.01f;
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed);
        transform.position += move * currentSpeed * Time.deltaTime;
    }

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        // Implement height adjustment for crouching
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0) health = 0;
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > 100) health = 100;
    }
}

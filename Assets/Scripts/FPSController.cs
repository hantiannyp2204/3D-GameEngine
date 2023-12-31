using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : UnityEngine.MonoBehaviour
{
    public CameraScipt cameraScript;
    public float moveSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraRotation();

    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraScript.transform.forward;
        Vector3 right = cameraScript.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }


    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        horizontalRotation += mouseX;

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        transform.Rotate(0, mouseX, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

   
}
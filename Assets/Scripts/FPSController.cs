using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera Camera;
    //public Rigidbody m_rb;
    public CharacterController CharacterController;
    public float MoveSpeed = 3.0f;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    // Public variables for customization
    //public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    // Private variables
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Ground check variables
    public float groundDistance = 1f; 
    public LayerMask groundMask;

    void Start()
    {
        // Getting the CharacterController component
        controller = GetComponent<CharacterController>();
        Camera.transform.position = this.transform.position + new Vector3(0,1,0);
        Camera.transform.SetParent(this.transform);
        Cursor.lockState = CursorLockMode.Locked;
 
    }

    void Update()
    {

        // Reset the velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get player input (WASD or Arrow keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the character
        controller.Move(move * MoveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the character based on velocity
        controller.Move(velocity * Time.deltaTime);

        RotatePlayer();
    }

    void RotatePlayer()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally (Y-axis) based on the mouse's X movement
        this.transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (X-axis) based on the mouse's Y movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the rotation to avoid full 360-degree flips

        // Apply the rotation to the camera
        Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}

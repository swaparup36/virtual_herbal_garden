using UnityEngine;

public class AndroidPlayerScrip : MonoBehaviour
{
    // Public variables
    public float moveSpeed = 5f;         // Movement speed
    public float jumpHeight = 2f;        // Jump height
    public float gravity = -9.81f;       // Gravity force
    public float joystickSensitivity = 1f;  // Sensitivity for the joystick movement

    // Private variables
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Ground check variables
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Joystick
    private Vector2 joystickDirection; // Holds the direction of the joystick input

    void Start()
    {
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        // Reset velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Handle movement based on touch input
        HandleTouchInput();

        // Move the player based on joystick input
        MovePlayer();

        // Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;

        // Move the character vertically
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleTouchInput()
    {
        // Check if the screen is being touched
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Calculate the movement direction based on the touch delta position (swiping)
                joystickDirection.x = touch.deltaPosition.x * joystickSensitivity * Time.deltaTime;
                joystickDirection.y = touch.deltaPosition.y * joystickSensitivity * Time.deltaTime;
            }
        }
        else
        {
            // No touch input, reset the joystick direction
            joystickDirection = Vector2.zero;
        }
    }

    void MovePlayer()
    {
        // Translate joystick direction into world space movement
        Vector3 move = transform.right * joystickDirection.x + transform.forward * joystickDirection.y;

        // Move the player
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

}

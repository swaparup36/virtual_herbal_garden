using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform Transform;
    public float scale = 5.02f;
    public float ScrollSensitivity = 150.0f;
    private Vector3 lastMousePosition;
    private float xRotation = 0f;

    public void OnGoBack()
    {
        SceneManager.LoadScene("Interaction Page");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnZoomIn()
    {
        scale += 0.1f;
        Transform.localScale = Vector3.one * scale;
    }

    public void OnZoomOut()
    {
        scale -= 0.1f;
        Transform.localScale = Vector3.one * scale;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(scrollInput);
        scale += scrollInput * ScrollSensitivity * Time.deltaTime;
        Transform.localScale = Vector3.one * scale;
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1)) // 0 is left, 1 is right, 2 is middle
        {
            // Get the mouse delta movement (difference between current and last frame)
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            // Apply the rotation to the object
            float xRotation = mouseDelta.y * mouseSensitivity * Time.deltaTime; // Rotation on the X axis
            float yRotation = -mouseDelta.x * mouseSensitivity * Time.deltaTime; // Rotation on the Y axis

            // Rotate the object based on the mouse movement
            Transform.Rotate(Vector3.up, yRotation, Space.World);  // Rotate around Y axis (horizontal)
            Transform.Rotate(Vector3.right, xRotation, Space.Self); // Rotate around X axis (vertical)
        }

        // Update the last mouse position for the next frame
        lastMousePosition = Input.mousePosition;
    }
}

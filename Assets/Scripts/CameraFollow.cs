using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform component
    public float cameraSpeed = 1f; // Speed at which the camera follows the player

    private Vector3 initialPosition; // Initial position of the camera relative to the player

    private Vector3 initialOffset;  

    private Vector3 lockedPosition;


    private void Start()
    {
        // Calculate the initial position of the camera relative to the player
        initialPosition = transform.position - player.position;

        Vector3 leftViewportPosition = new Vector3(0f, 0.5f, Camera.main.nearClipPlane);
        Vector3 rightViewportPosition = new Vector3(1f, 0.5f, Camera.main.nearClipPlane);

    }

    private void LateUpdate()
    {

        // Calculate the target position of the camera (only consider the player's x-axis position)
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z) + initialOffset;

        // Interpolate the camera's position towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);

        Vector3 cameraPosition = Camera.main.transform.position;
    }
}
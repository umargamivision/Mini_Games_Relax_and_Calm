using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;           // The chicken GameObject to follow
    public float smoothSpeed = 0.125f; // The speed at which the camera catches up
    public float xOffset = 0f;         // Offset for the X-axis position of the camera

    private void LateUpdate()
    {
        if (target != null)
        {
            // Desired position with target's X position plus offset, and camera's existing Y and Z positions
            Vector3 desiredPosition = new Vector3(target.position.x + xOffset, transform.position.y, transform.position.z);
            // Smoothly interpolate between the current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Update the camera position
            transform.position = smoothedPosition;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HorizontalSwipeController : MonoBehaviour
{
    public GameObject spriteToMove;     // Assign the sprite GameObject here in the Inspector
    public float moveSpeed = 0.1f;      // Adjust the speed of horizontal movement
    public float minX = -5f;            // Set the minimum X boundary
    public float maxX = 5f;             // Set the maximum X boundary

    private Vector2 lastTouchPosition;
    private bool isSwiping = false;
    private Rigidbody2D rb;             // Reference to the Rigidbody2D component

    void Start()
    {
        // Get the Rigidbody2D component attached to spriteToMove
        rb = spriteToMove.GetComponent<Rigidbody2D>();

        // Ensure gravity scale is initially set to 0
        if (rb != null)
        {
            rb.gravityScale = 0;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is over a UI element; if so, ignore it
            if (IsTouchOverUI(touch))
            {
                return;
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Start tracking the swipe
                    lastTouchPosition = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        // Calculate the swipe distance in the horizontal (X) direction
                        Vector2 currentTouchPosition = touch.position;
                        float deltaX = currentTouchPosition.x - lastTouchPosition.x;

                        // Move the sprite only in the X direction
                        float newXPosition = spriteToMove.transform.position.x + deltaX * moveSpeed * Time.deltaTime;

                        // Clamp the new position within the bounds
                        newXPosition = Mathf.Clamp(newXPosition, minX, maxX);

                        // Set the sprite's new position
                        spriteToMove.transform.position = new Vector3(newXPosition, spriteToMove.transform.position.y, spriteToMove.transform.position.z);

                        // Update the last touch position
                        lastTouchPosition = currentTouchPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // Stop tracking the swipe
                    isSwiping = false;

                    // Set gravity scale to 50 when the user lifts their finger
                    if (rb != null)
                    {
                        rb.gravityScale = 50;
                    }
                    break;
            }
        }
    }

    // Method to check if the touch is over a UI element
    private bool IsTouchOverUI(Touch touch)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = touch.position
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        return raycastResults.Count > 0;
    }
}

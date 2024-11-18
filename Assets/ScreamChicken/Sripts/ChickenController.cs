using UnityEngine;
using UnityEngine.SceneManagement; // Needed to reload the scene
using System.Collections;

public class ChickenController : MonoBehaviour
{
   // public GameObject levelComplete;
   // public GameObject levelFailed;
    public float jumpForce = 10f;
    public float minJumpForce = 5f; // Minimum jump force
    public float maxJumpTime = 2f;
    public float minRightwardSpeed = 2f; // Minimum rightward speed
    public float maxRightwardSpeed = 10f; // Maximum rightward speed
    public string groundLayerName = "Ground";  // Name of the ground layer in Unity
    public string gameOverLayerName = "GameOver";  // Name of the GameOver layer in Unity
    public string levelCompleteLayerName = "Win";  // Name of the GameOver layer in Unity
    private Rigidbody2D rb;
    private float jumpTime;
    private bool isJumping;
    public bool isOnGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detect when the player starts holding the screen
        if (Input.GetMouseButtonDown(0) && isOnGround)
        {
            StartJump();
        }

        // Detect continuous press for jump power up to a max limit
        if (Input.GetMouseButton(0) && isJumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime <= maxJumpTime)
            {
                // Calculate the rightward force based on jumpTime
                float currentRightwardSpeed = Mathf.Lerp(minRightwardSpeed, maxRightwardSpeed, jumpTime / maxJumpTime);

                // Apply upward and rightward force as long as jumpTime < maxJumpTime
                rb.velocity = new Vector2(currentRightwardSpeed, Mathf.Max(minJumpForce, jumpForce * jumpTime));
            }
            else
            {
                // If max jump limit reached, force the chicken to descend
                isJumping = false;
            }
        }

        // Detect when the user releases the press
        if (Input.GetMouseButtonUp(0))
        {
            isJumping = false;
        }

        // When chicken lands, check if the player is holding to jump again
        if (isOnGround && !isJumping)
        {
            rb.velocity = Vector2.zero; // Reset velocity when on the ground

            if (Input.GetMouseButton(0))
            {
                StartJump(); // Start a new jump if the screen is still pressed
            }
        }
    }

    void StartJump()
    {
        jumpTime = 0f;
        isJumping = true;
        // Initial jump with minimum force applied to ensure a noticeable jump
        rb.velocity = new Vector2(minRightwardSpeed, minJumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the chicken is colliding with the ground layer
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            isOnGround = true;
            Debug.Log("Chicken is on the ground");
        }

        // Check if the chicken is colliding with the GameOver layer
        if (collision.gameObject.layer == LayerMask.NameToLayer(gameOverLayerName))
        {
            ScreamManager.instance.LevelFail();
           // levelFailed.SetActive(true);
            Debug.Log("Game Over triggered - Restarting scene");
            // StartCoroutine(RestartSceneAfterDelay());
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer(levelCompleteLayerName))
        {
            Debug.Log("Level complete triggered - Restarting scene");
            // levelComplete.SetActive(true);
            ScreamManager.instance.LevelComplete();
            // StartCoroutine(RestartSceneAfterDelay());
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the chicken is no longer touching the ground layer
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            isOnGround = false;
            Debug.Log("Chicken left the ground");
        }
    }
}

    // Coroutine to wait and restart the scene
  

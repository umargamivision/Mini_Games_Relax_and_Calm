using UnityEngine;
using UnityEngine.SceneManagement; // Needed to reload the scene
using System.Collections;
using DG.Tweening;

public class ChickenController : MonoBehaviour
{
    // public GameObject levelComplete;
    // public GameObject levelFailed;
    public bool throughForce;
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
        if (!throughForce &&isOnGround && !isJumping)
        {
            rb.velocity = Vector2.zero; // Reset velocity when on the ground

            if (Input.GetMouseButton(0))
            {
                StartJump(); // Start a new jump if the screen is still pressed
            }
        }
        if(throughForce && isOnGround)
        {
            if(Input.GetMouseButton(0))
            {
                Jump(new Vector2(0, minJumpForce),1);
            }
        }
        if(throughForce)
        {
            if(Input.GetMouseButton(0))
            {
                //xVelocityFactor = Mathf.Lerp(rb.velocity.x, xVelocityFactor, 0.5f);
                rb.velocity=new Vector2(minRightwardSpeed, rb.velocity.y);
            }
            else
            {
                xVelocityFactor = Mathf.Lerp(rb.velocity.x, 0, tFactorOfXVelocity*Time.deltaTime);
                rb.velocity=new Vector2(xVelocityFactor, rb.velocity.y);
            }
        }
    }
    public float tFactorOfXVelocity;
            float xVelocityFactor=0;

    void StartJump()
    {
        // jumpTime = 0f;
        // isJumping = true;
        // // Initial jump with minimum force applied to ensure a noticeable jump
        // rb.velocity = new Vector2(minRightwardSpeed, minJumpForce);
    }
    void Jump(Vector2 jumpDirect, float force)
    {
        jumpTime = 0f;
        isJumping = true;
        // Initial jump with minimum force applied to ensure a noticeable jump
        rb.AddForce(jumpDirect*force,ForceMode2D.Impulse);
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
            ScreamManager.instance.OnLevelFail();
            // levelFailed.SetActive(true);
            Debug.Log("Game Over triggered - Restarting scene");
            // StartCoroutine(RestartSceneAfterDelay());
        }
        if (collision.gameObject.name.Contains("bridge"))
        {

            float bridgeCenterX = collision.collider.bounds.center.x;

            // Get the first contact point of the collision
            Vector2 contactPoint = collision.contacts[0].point;

            // Check whether the contact point is on the left or right
            if (contactPoint.x > bridgeCenterX)
            {
                //Debug.Log("Collision occurred on the RIGHT side of the bridge.");
                BreakBridge(collision.transform, true);
            }
            else
            {
                //Debug.Log("Collision occurred on the LEFT side of the bridge.");
                BreakBridge(collision.transform, false);
            }

            //collision.transform.isdo

        }
        if(collision.gameObject.name.Contains("RJumper"))
        {
            Jump(new Vector2(0,minJumpForce),-1.5f);
        }
        else if(collision.gameObject.name.Contains("Jumper"))
        {
            Jump(new Vector2(0,minJumpForce),1.5f);
        }

    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.name.Contains("RJumper"))
        {
            Jump(new Vector2(0,minJumpForce),-2);
        }
        else if(other.gameObject.name.Contains("Jumper"))
        {
            Jump(new Vector2(0,minJumpForce),2);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer(levelCompleteLayerName))
        {
            Debug.Log("Level complete triggered - Restarting scene");
            // levelComplete.SetActive(true);
            ScreamManager.instance.OnLevelComplete();
            // StartCoroutine(RestartSceneAfterDelay());
        }
    }
    public void BreakBridge(Transform collision, bool rightSide)
    {
        print("collide right side " + rightSide);
        if (DOTween.IsTweening(collision.transform)) return;
        float rotate = rightSide ? -10 : 10;
        Sequence sequence = DOTween.Sequence();
        Tween tween0 = collision.transform.DORotate(Vector3.forward * rotate, 0.4f).SetDelay(0).SetRelative(true);
        Tween tween1 = collision.transform.DORotate(Vector3.forward * -rotate, 0.3f).SetDelay(0).SetRelative(true);
        Tween tween2 = collision.transform.DOMoveY(-10, 5).SetRelative(true);
        sequence.Append(tween0);
        sequence.Append(tween1);
        sequence.Append(tween2);
        sequence.Play();
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


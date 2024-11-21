using UnityEngine;
using System.Collections;

public class ChickenControllerVoice : MonoBehaviour
{
    public GameObject levelComplete;
    public GameObject levelfailed;
    public float jumpForce = 10f;
    public float minJumpForce = 5f;
    public float maxJumpTime = 2f;
    public float minRightwardSpeed = 2f;
    public float maxRightwardSpeed = 10f;
    public string groundLayerName = "Ground";
    public string gameOverLayerName = "GameOver";
    public string levelCompleteLayerName = "Win";
    public int sampleWindow = 128;
    public float volumeThreshold = 0.02f; // Minimum sound level to detect (adjust as needed)

    private Rigidbody2D rb;
    private float jumpTime;
    private bool isJumping;
    private bool isOnGround;
    private AudioClip micClip;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartMicrophone();
    }

    void Update()
    {
        if (IsSoundDetected())
        {
            if (!isJumping && isOnGround)
            {
                StartJump();
            }
        }
        else
        {
            isJumping = false;
        }

        if (isJumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime <= maxJumpTime)
            {
                float currentRightwardSpeed = Mathf.Lerp(minRightwardSpeed, maxRightwardSpeed, jumpTime / maxJumpTime);
                rb.velocity = new Vector2(currentRightwardSpeed, Mathf.Max(minJumpForce, jumpForce * jumpTime));
            }
            else
            {
                isJumping = false;
            }
        }

        if (isOnGround && !isJumping)
        {
            rb.velocity = Vector2.zero;

            if (isJumping)
            {
                StartJump();
            }
        }
    }

    private void StartJump()
    {
        jumpTime = 0f;
        isJumping = true;
        rb.velocity = new Vector2(minRightwardSpeed, minJumpForce);
    }

    private void StartMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            micClip = Microphone.Start(null, true, 1, 44100); // Start microphone
        }
        else
        {
            Debug.LogWarning("No microphone detected!");
        }
    }

    private bool IsSoundDetected()
    {
        if (micClip == null)
            return false;

        float[] samples = new float[sampleWindow];
        micClip.GetData(samples, Microphone.GetPosition(null) - sampleWindow);

        float sum = 0;
        foreach (var sample in samples)
        {
            sum += Mathf.Abs(sample); // Calculate the average volume level
        }

        float averageVolume = sum / sampleWindow;
        return averageVolume > volumeThreshold; // Only detect if above threshold
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            isOnGround = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer(gameOverLayerName))
        {
            levelfailed.SetActive(true);
           // ScreamManager.instance.RestartLevel();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer(levelCompleteLayerName))
        {
            Debug.Log("Level complete triggered - Restarting scene");
            levelComplete.SetActive(true);

            // StartCoroutine(RestartSceneAfterDelay());
        }
        if (collision.gameObject.CompareTag("Level1"))
        {
            Debug.Log("Level complete triggered - Restarting scene");
            PlayerPrefs.SetInt("ScreamChicken_Level", 2);
            ScreamManager.instance.LevelComplete();
        }
        if (collision.gameObject.CompareTag("Level2"))
        {
            Debug.Log("Level complete triggered - Restarting scene");
            PlayerPrefs.SetInt("ScreamChicken_Level", 3);
            ScreamManager.instance.LevelComplete();
        }
        if (collision.gameObject.CompareTag("Level3"))
        {
            Debug.Log("Level complete triggered - Restarting scene");
            PlayerPrefs.SetInt("ScreamChicken_Level", 4);
            ScreamManager.instance.LevelComplete();
        }
        if (collision.gameObject.CompareTag("Level4"))
        {
            Debug.Log("Level complete triggered - Restarting scene");
            PlayerPrefs.SetInt("ScreamChicken_Level", 5);
            ScreamManager.instance.LevelComplete();
        }
    }
  
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            isOnGround = false;
        }
    }

   

    void OnDestroy()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
        }
    }
}

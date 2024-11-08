using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HamsterPlayerController : MonoBehaviour
{
    public float jumpForce;
    public Vector2 jumpDirection;
    public Rigidbody2D rb;
    public Collider2D coll2D;
    public PhysicsMaterial2D bouncePM, frictionlessPM;
    public UnityEvent<int> onAddScore;
    public UnityEvent onFail;
    private void Start() 
    {
        coll2D.sharedMaterial=bouncePM;
    }
    public void Jump(bool right)
    {
        // rb.Sleep();
        // rb.WakeUp();
        if (rb.isKinematic) rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        if (right)
            rb.AddForce(new Vector2(jumpDirection.x, jumpDirection.y) * jumpForce * Time.deltaTime, ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(-jumpDirection.x, jumpDirection.y) * jumpForce * Time.deltaTime, ForceMode2D.Impulse);
        //rb.AddForce((Vector2.up + Vector2.left) * jumpForce);
    }
    public void ScreenClamp()
    {
        var screenPointPosition = Camera.main.WorldToScreenPoint(transform.position);
        var clampedPosition = new Vector3(Mathf.Clamp(screenPointPosition.x, 0, Screen.width), screenPointPosition.y, screenPointPosition.z);
        transform.position = Camera.main.ScreenToWorldPoint(clampedPosition);
    }
    public void JumpInputs()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x > Screen.width / 2)
                {
                    // Right side tap detected
                    Debug.Log("Right side tap");
                    Jump(true);
                    // Add right tap logic here (e.g., move player right)
                }
                else
                {
                    Jump(false);
                    // Left side tap detected
                    Debug.Log("Left side tap");
                    // Add left tap logic here (e.g., move player left)
                }
            }
        }

        // Handle mouse input (for testing on computer)
        if (Input.GetMouseButtonDown(0))  // 0 is the left mouse button
        {
            Vector3 mousePosition = Input.mousePosition;

            if (mousePosition.x > Screen.width / 2)
            {
                // Right side click detected
                Debug.Log("Right side click");
                Jump(true);
                // Add right click logic here (e.g., move player right)
            }
            else
            {
                // Left side click detected
                Debug.Log("Left side click");
                Jump(false);
                // Add left click logic here (e.g., move player left)
            }
        }
    }
    void Update()
    {
        JumpInputs();
        ScreenClamp();
        CheckFail();
    }
    public void CheckFail()
    {
        if(Camera.main.WorldToScreenPoint(transform.position).y < 0)
        {
            OnFail();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        print("bounciness is "+coll2D.bounciness);
        if (other.transform.name.Contains("Platform"))
        {
            OnFail();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.transform.name.Contains("ScorePoint"))
        {
            other.enabled=false;
            onAddScore.Invoke(1);
        }
    }
    public void OnFail()
    {
        coll2D.sharedMaterial = frictionlessPM;
        onFail.Invoke();
        this.enabled = false;
    }
}
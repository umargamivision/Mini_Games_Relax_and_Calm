using System;
using System.Collections;
using UnityEngine;

public class RedLightPlayer : MonoBehaviour
{

    public enum State { die, alive, win }
    public State currentState;
    public SpriteRenderer spriteRenderer;
    public Joystick joystick;
    public RedLightBoss redLightBoss;
    public Transform finishPoint;
    public float speed;
    private void Update()
    {
        if (currentState == State.alive)
            Movement();
        if ((finishPoint.position.y - transform.position.y) < 0 && currentState == State.alive)
            Win();
        // Check if the light is red and the player is moving, only if they are not already dead
        if (redLightBoss.lightColor == RedLightBoss.LightColor.red && !(currentState == State.die))
        {
            if (new Vector2(joystick.Horizontal, joystick.Vertical).magnitude > 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        currentState = State.die;
        spriteRenderer.color = Color.red;
        StartCoroutine(DelyAction(RedLightMiniGame.Instance.LevelFail, 1));
    }
    public void Win()
    {
        currentState = State.win;
        spriteRenderer.color = Color.blue;
        StartCoroutine(DelyAction(RedLightMiniGame.Instance.LevelComplete, 1));
    }
    IEnumerator DelyAction(Action _action, float dely)
    {
        yield return new WaitForSeconds(dely);
        _action.Invoke();
    }
    public void Movement()
    {
        if (currentState == State.die) return; // Prevent movement if the player is dead

        // Move the player based on joystick input
        Vector2 move = new Vector2(joystick.Horizontal, joystick.Vertical) * speed * Time.deltaTime;
        transform.Translate(move);

        // Get the player's current position in screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Clamp the player's screen position to keep it within the screen bounds
        screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
        screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

        // Convert back to world space and set the player's position
        transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
    }

    // Call this method to reset the player state if necessary
    public void ResetPlayer()
    {
        currentState = State.alive;
        spriteRenderer.color = Color.white; // Restore original color
    }
}
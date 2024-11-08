
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RedLightAI : MonoBehaviour
{
    public enum State {die, alive, win}
    public State currentState;
    public SpriteRenderer spriteRenderer;
    public RedLightBoss redLightBoss;
    public Transform finishPoint;
    public float speed;

    private void Update()
    {
        if(currentState==State.alive)
            Movement();
        if((finishPoint.position.y-transform.position.y)<0)
            Win();
        if (redLightBoss.lightColor == RedLightBoss.LightColor.red && !(currentState==State.die))
        {
            if (new Vector2(AIJoyStick().x, AIJoyStick().y).magnitude > 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        currentState = State.die; 
        spriteRenderer.color = Color.red; 
    }
    public void Win()
    {
        currentState=State.win;
        spriteRenderer.color = Color.blue;
    }
    public Vector2 AIJoyStick()
    {
        if (redLightBoss.lightColor == RedLightBoss.LightColor.red)
        {
            return Vector2.zero;
        }
        else
        {
            
            float horizontalMovement = Mathf.PerlinNoise(Time.time, 0) * 2 - 1; 
            float verticalMovement = 1; 

            return new Vector2(horizontalMovement, verticalMovement);
        }
    }
    public void Movement()
    {
        if (currentState==State.die) return; 
        Vector2 move = new Vector2(AIJoyStick().x, AIJoyStick().y) * speed * Time.deltaTime;
        transform.Translate(move);

      
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        
        screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
        screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

        
        transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
    }

   
    public void ResetPlayer()
    {
        currentState = State.alive;
        spriteRenderer.color = Color.white; 
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
namespace RedLightMiniGameSpace
{
    public class RedLightAI : Player
    {
        //public Rigidbody2D rigidbody2D;
        private void Update()
        {
            // if (currentState == State.alive)
            //     RBMovement();
            if ((finishPoint.position.y - transform.position.y) < 0)
                Win();
            if (redLightBoss.lightColor == RedLightBoss.LightColor.red && !(currentState == State.die))
            {
                if (new Vector2(AIJoyStick().x, AIJoyStick().y).magnitude > 0)
                {
                    Die();
                }
            }
        }
        private void FixedUpdate()
        {
            if (currentState == State.alive)
                RBMovement();
        }
        public void Die()
        {
            currentState = State.die;
            spriteRenderer.color = Color.red;
        }
        public void Win()
        {
            currentState = State.win;
            spriteRenderer.color = Color.blue;
            rb2D.velocity = Vector3.zero;
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
        public void RBMovement()
        {
            if (currentState == State.die) return; // Prevent movement if the player is dead

            // Move the player based on joystick input
            Vector2 move = new Vector2(AIJoyStick().x, AIJoyStick().y * speed);
            rb2D.velocity = move;

            // Get the player's current position in screen space
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            // Clamp the player's screen position to keep it within the screen bounds
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

            // Convert back to world space
            Vector3 clampedWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // Set the Rigidbody2D position, clamping only the position
            rb2D.position = new Vector2(clampedWorldPosition.x, clampedWorldPosition.y);
        }
        public void Movement()
        {
            if (currentState == State.die) return;
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
}
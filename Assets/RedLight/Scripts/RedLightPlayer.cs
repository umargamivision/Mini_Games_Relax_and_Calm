using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Ommy.Audio;
namespace RedLightMiniGameSpace
{
    public class RedLightPlayer : Player
    {
        //public Rigidbody2D rigidbody2D;
        public GameObject sheild;
        public Joystick joystick;
        public GameObject[] characters;
        private void Start() 
        {
            SetCharacter();
        }
        public void SetCharacter(int index=0)
        {
            characters.ToList().ForEach(f=>f.SetActive(false));
            characters[index].SetActive(true);
            spriteRenderer = characters[index].GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            // if (currentState == State.alive)
            //     RBMovement();
            if ((finishPoint.position.y - transform.position.y) < 0 && currentState == State.alive)
                Win();
            // Check if the light is red and the player is moving, only if they are not already dead
            if (redLightBoss.lightColor == RedLightBoss.LightColor.red && !(currentState == State.die))
            {
                if (new Vector2(joystick.Horizontal, joystick.Vertical).magnitude > 0)
                {
                    KillPlayer();
                }
            }
        }
        private void FixedUpdate() 
        {
            if (currentState == State.alive)
                RBMovement();    
            else
                rb2D.velocity=Vector3.zero;
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
        public void RBMovement()
        {
            if (currentState == State.die) return; // Prevent movement if the player is dead

            // Move the player based on joystick input
            Vector2 move = new Vector2(joystick.Horizontal, joystick.Vertical) * speed;
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
        public override void KillPlayer()
        {
            if(sheild.activeInHierarchy) return;
            Die();
            AudioManager.Instance.PlaySFX(deathClip);
        }
        public void ApplySheildClick()
        {
            AdsManager.ShowRewardedAd(()=>
            {
                ApplySheild();
            }, "RedLight Sheild");
        }
        void ApplySheild()
        {
            StartCoroutine(ApplySheildDely());
        }
        IEnumerator ApplySheildDely()
        {
            sheild.SetActive(true);
            yield return new WaitForSeconds(3);
            sheild.SetActive(false);
        }
    }
    public class Player : MonoBehaviour
    {
        public enum State { die, alive, win }
        public float speed;
        public Rigidbody2D rb2D;
        public State currentState;
        public SpriteRenderer spriteRenderer;
        public RedLightBoss redLightBoss;
        public Transform finishPoint;
        public AudioClip deathClip;
        public void ApplyTrap()
        {

        }
        public virtual void KillPlayer()
        {
            print("Player Die");
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using UnityEngine;
using DG.Tweening;
namespace RedLightMiniGameSpace
{
    public class BombTrap : RedLightTrap
    {
        public float explosionRadius = 5.0f;
        public float explosionDelay = 3.0f;
        public float dropSpeed;
        public float dely;
        public Transform dropPoint;
        public GameObject explosionEffect;

        private void Start() 
        {
            transform.DOMove(dropPoint.position,dropSpeed).OnComplete(()=>TriggerTrap(null)).SetEase(Ease.OutSine).SetDelay(dely);     
        }
        public override void TriggerTrap(Player player)
        {
            // Start the explosion countdown
            StartCoroutine(ExplodeAfterDelay());
        }

        private IEnumerator ExplodeAfterDelay()
        {
            // Optional: Show warning (like a blinking light or countdown sound)
            yield return new WaitForSeconds(explosionDelay);

            // Check if the player is within the explosion radius
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position, explosionRadius);
            bool playerInRange = false;

            foreach (Collider2D hit in hits)
            {
                var player = hit.GetComponent<Player>();
                if (player!=null)
                {
                    playerInRange = true;
                    player.KillPlayer();
                    break;
                }
            }
            // Optional: Instantiate explosion effect
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);

            // Destroy bomb after explosion
            
        }
    }
}
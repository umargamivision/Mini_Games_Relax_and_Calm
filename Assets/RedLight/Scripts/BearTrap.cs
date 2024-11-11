using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace RedLightMiniGameSpace
{
    public class BearTrap : RedLightTrap
    {
        public float trapTime;
        public override void TriggerTrap(Player player)
        {
            this.player = player;
            StartCoroutine(ApplyTrap());
        }
        public IEnumerator ApplyTrap()
        {
            var tempSpeed = player.speed;
            player.speed = 0;
            player.transform.DOMove(transform.position,0.5f);
            yield return new WaitForSeconds(trapTime);
            player.speed = tempSpeed;
        }
        private void OnTriggerEnter2D(Collider2D other) 
        {
            print("Trap Trigger");
            var player = other.GetComponent<Player>();
            if(player!=null)
            {
                TriggerTrap(player);
            }    
        }
    }
}
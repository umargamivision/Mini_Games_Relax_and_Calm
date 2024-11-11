using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RedLightMiniGameSpace;
using UnityEngine;
namespace RedLightMiniGameSpace
{
    public class BananaPeelTrap : RedLightTrap
    {
        private float trapTime;

        public override void TriggerTrap(Player _player)
        {
            player = _player;
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

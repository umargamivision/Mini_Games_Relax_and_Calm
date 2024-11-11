using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
namespace RedLightMiniGameSpace
{
    public class TeleportTrap : RedLightTrap
    {
        public float trapTime;
        public Transform outPoint;
        public override void TriggerTrap(Player _player)
        {
            player = _player;
            StartCoroutine(ApplyTrap());
        }
        public IEnumerator ApplyTrap()
        {
            var tempSpeed = player.speed;
            player.speed = 0;
            player.transform.DOMove(transform.position,0.5f).OnComplete(()=>player.gameObject.SetActive(false));
            yield return new WaitForSeconds(trapTime);
            player.transform.position = outPoint.position;
            player.gameObject.SetActive(true);
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

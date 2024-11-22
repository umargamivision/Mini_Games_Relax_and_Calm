using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ommy.Audio;
using RedLightMiniGameSpace;
using UnityEngine;
namespace RedLightMiniGameSpace
{
    public class BananaPeelTrap : RedLightTrap
    {
        public float trapTime;
        public AudioClip trapClip;
        public override void TriggerTrap(Player _player)
        {
            player = _player;
            StartCoroutine(ApplyTrap());
            AudioManager.Instance.PlaySFX(trapClip);
        }
         public IEnumerator ApplyTrap()
        {
            var tempSpeed = player.speed;
            player.speed = 0;
            player.rb2D.isKinematic=true;
            player.transform.DOMove(transform.position,0.5f);
            yield return new WaitForSeconds(trapTime);
            player.rb2D.isKinematic=false;
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

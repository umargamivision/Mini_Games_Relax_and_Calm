using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RedLightMiniGameSpace
{
    public abstract class RedLightTrap : MonoBehaviour
    {
        public Player player;
        public abstract void TriggerTrap(Player gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace RedLightMiniGameSpace
{
    public abstract class RedLightTrap : MonoBehaviour
    {
        public Player player;
        public abstract void TriggerTrap(Player gameObject);
    }
}
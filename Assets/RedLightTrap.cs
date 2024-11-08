using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightTrap : MonoBehaviour
{
    public enum TrapType
    {
        bananaPeel,
        bearTrap,
        bomb,
        teleport
    }
    public TrapType trapType;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.name.Contains("Player"))
        {
        
        }    
    }
}

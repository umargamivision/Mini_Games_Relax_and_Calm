using System.Collections;
using System.Collections.Generic;
using DoReMiSpace;
using UnityEngine;
using UnityEngine.Events;

public class PlatformDoReMe : MonoBehaviour
{
    public int level;
    public UnityEvent onPlayerPassed;
    public void OnPlayerPassed()
    {
        onPlayerPassed.Invoke();
        gameObject.SetActive(false);
    }
}

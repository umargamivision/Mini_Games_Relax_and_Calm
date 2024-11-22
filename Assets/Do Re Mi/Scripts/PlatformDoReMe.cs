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
        Invoke(nameof(SetActiveDely),2);
        //gameObject.SetActive(false);
    }
    void SetActiveDely()
    {
        gameObject.SetActive(false);
    }
}

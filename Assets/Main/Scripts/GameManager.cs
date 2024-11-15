using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        Application.targetFrameRate = 30;
    }
}
public enum MiniGame
{
    RedLight,
    DoReMi,
    HamsterJump,
    PopIt,
    CutInHalf,
    ScreamChicken
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopItMiniGame : MiniGameBase
{
    public int currentLevel;
    public GameObject[] levels;
    private void Start() 
    {
        SetLevel(currentLevel);
    }
    public override void NextLevel()
    {
        throw new System.NotImplementedException();
    }

    public override void ReplayLevel()
    {
        throw new System.NotImplementedException();
    }
    public void OnCompleteLevel()
    {
        currentLevel++;
        if(currentLevel>=levels.Length)
        {
            currentLevel=0;
        }
        SetLevel(currentLevel);
    }
    public void SetLevel(int level)
    {
        foreach (var item in levels)
        {
            item.SetActive(false);
        }
        levels[level].SetActive(true);
    }
}

using System;
using Ommy.SaveData;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    public MiniGameData miniGameData;
    public void SaveMiniGameData()
    {
        var data = SaveData.Instance.miniGamesData.Find(f => f.miniGame == miniGameData.miniGame);
        if (data == null)
        {
            SaveData.Instance.miniGamesData.Add(miniGameData);
            data = SaveData.Instance.miniGamesData.Find(f => f.miniGame == miniGameData.miniGame);
        }
        data = miniGameData;
        SaveSystem.SaveProgress();
    }
    public void FetchMiniGameData()
    {
        SaveSystem.LoadProgress();
        miniGameData = SaveData.Instance.miniGamesData.Find(f => f.miniGame == miniGameData.miniGame);
    }
    public void AddFavourite(bool farourite)
    {
        if (farourite)
            //SaveData.Instance.miniGamesData.ForEach(f=>f.) 
            miniGameData.favouriteIndex = 1;
        else
            miniGameData.favouriteIndex = -1;
    }
    // for testing
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveMiniGameData();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            FetchMiniGameData();
        }
    }
    public virtual void MiniGameStart()
    {
        Application.targetFrameRate = 30;
        Debug.Log("GAME START");
    }
    public virtual void LevelComplete()
    {
        Debug.Log("GAME COMPLETE");
        UIManager.Instance.ShowLevelComplete();
    }
    public virtual void LevelFail()
    {
        Debug.Log("GAME FAIL");
        UIManager.Instance.ShowLevelFail();
    }
    public abstract void NextLevel();
    public abstract void ReplayLevel();
}
[Serializable]
public class MiniGameData
{
    public MiniGame miniGame;
    public int favouriteIndex = -1;
    public int currentLevel = 1;
    public int highScore = 0;
    public int currentTheme;
    public int currentCharacter;
}
using System;
using System.Linq;
using Ommy.SaveData;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    public CaptureScreen captureScreen;
    public MiniGameData miniGameData;
    public void Start() 
    {
        FetchMiniGameData();    
    }
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
        var mGD = SaveData.Instance.miniGamesData.Find((f => f.miniGame == miniGameData.miniGame));
        if(mGD!=null)
            miniGameData = mGD; 
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
        FetchMiniGameData();
        Application.targetFrameRate = 30;
        Debug.Log("GAME START");
    }
    public virtual void LevelComplete()
    {
        SaveMiniGameData();
        Debug.Log("GAME COMPLETE");
        UIManager.Instance.ShowLevelComplete();
    }
    public virtual void LevelFail()
    {
        SaveMiniGameData();
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
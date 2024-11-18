using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Ommy.Prefs;
using Ommy.SaveData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public MiniGameBase miniGameBase;
    public SceneLoader sceneLoader;
    public GameObject levelCompleteScreen;
    public GameObject levelFailScreen;
    public GameObject loadingScreen;
    public GameObject settingScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        if(miniGameBase == null) miniGameBase = FindObjectOfType<MiniGameBase>();
    }
    public void AddFavourite(bool farourite)
    {
        if(farourite)
            miniGameBase.miniGameData.favouriteIndex = -1;
        else
            miniGameBase.miniGameData.favouriteIndex = -1;
    }
    public void NextLevelClick()
    {
        miniGameBase.NextLevel();
    }
    public void ReplayLevelClick()
    {
        miniGameBase.ReplayLevel();
    }
    public void ShowLevelComplete()
    {
        levelCompleteScreen.SetActive(true);
    }
    public void ShowLevelFail()
    {
        levelFailScreen.SetActive(true);
    }
    public void LoadMiniGameScene(string miniGameSceneName)
    {
        levelCompleteScreen.SetActive(false);
        levelFailScreen.SetActive(false);
        sceneLoader.LoadMiniGameScene(miniGameSceneName);
    }
    public void LoadMiniGameScene(MiniGame miniGame)
    {
        levelCompleteScreen.SetActive(false);
        levelFailScreen.SetActive(false);
        sceneLoader.LoadMiniGameScene(miniGame.ToString());
    }
    public void LoadMainMenu()
    {
        sceneLoader.LoadMainMenuScene();
    }
    public void SettingClick()
    {
        settingScreen.SetActive(true);
    }
    public void ResetPanels()
    {
        levelCompleteScreen.SetActive(false);
        levelFailScreen.SetActive(false);
        loadingScreen.SetActive(false);
        settingScreen.SetActive(false);
    }
}

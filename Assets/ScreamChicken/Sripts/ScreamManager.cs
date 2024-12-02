using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreamManager : MiniGameBase
{
    public GameObject controller; // UI Controller for pausing the game
    public static ScreamManager instance;
    public GameObject[] levels;

    void Start()
    {
        base.Start();
        MiniGameStart();
        instance = this;

        // Check if returning from MainMenuScene and pause the game
        int x = PlayerPrefs.GetInt("MainMenuScene", 0);
        if (x == 1)
        {
            Time.timeScale = 0;
            controller.SetActive(true);
            PlayerPrefs.SetInt("MainMenuScene", 0);
        }
        ActivateLevel(miniGameData.currentLevel);
    }
    public void ActivateLevel(int index)
    {
        foreach (var item in levels)
        {
            item.SetActive(false);
        }
        levels[index].SetActive(true);
    }
    public void OnLevelComplete()
    {
        miniGameData.currentLevel++;
        if(miniGameData.currentLevel>=5)
        {
            miniGameData.currentLevel=0;
        }
        Invoke(nameof(LevelComplete),1);
    }
    public void OnLevelFail()
    {
        Invoke(nameof(LevelFail),1);
    }
    public override void LevelFail()
    {
        base.LevelFail();
    }
    public override void LevelComplete()
    {
        base.LevelComplete();
    }
    public void RestartLevel()
    {
        StartCoroutine(RestartSceneAfterDelay());
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void NextLevel()
    {
        StartCoroutine(RestartSceneAfterDelay());
    }

    public override void ReplayLevel()
    {
        StartCoroutine(RestartSceneAfterDelay());
    }
}

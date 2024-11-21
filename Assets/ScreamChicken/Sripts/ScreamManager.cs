using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreamManager : MiniGameBase
{
    public GameObject controller; // UI Controller for pausing the game
    public static ScreamManager instance; // Singleton instance
    public GameObject levelsParent; // Parent GameObject for level prefabs
    public GameObject level1Prefab; // Prefab for level 1
    public GameObject level2Prefab; 
    public GameObject level3Prefab; 
    public GameObject level4Prefab; 

    void Start()
    {
        instance = this;

        // Check if returning from MainMenuScene and pause the game
        int x = PlayerPrefs.GetInt("MainMenuScene", 0);
        if (x == 1)
        {
            Time.timeScale = 0;
            controller.SetActive(true);
            PlayerPrefs.SetInt("MainMenuScene", 0);
        }

        // Load the saved level or default to level 1
        int currentLevel = PlayerPrefs.GetInt("ScreamChicken_Level", 1);
        Debug.Log("The current level is " + currentLevel);
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
        // Clear any existing levels under levelsParent
        foreach (Transform child in levelsParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the appropriate level prefab
        GameObject levelPrefab = null;
        switch (level)
        {
            case 1:
                levelPrefab = level1Prefab;
                break;
            case 2:
                levelPrefab = level2Prefab;
                break;
            case 3:
                levelPrefab = level3Prefab;
                break;
            case 4:
                levelPrefab = level4Prefab;
                break;
           
            default:
                Debug.LogError("Invalid level: " + level);
                return;
        }

        if (levelPrefab != null)
        {
            Instantiate(levelPrefab, levelsParent.transform);
        }
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

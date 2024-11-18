using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ScreamManager : MiniGameBase
{
    public GameObject controller;
    public static ScreamManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
       int x= PlayerPrefs.GetInt("MainMenuScene");
        if (x == 1)
        {
            Time.timeScale = 0;
            controller.SetActive(true);
            PlayerPrefs.SetInt("MainMenuScene", 0);
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
        //throw new System.NotImplementedException();
    }

    public override void ReplayLevel()
    {
        StartCoroutine(RestartSceneAfterDelay());
        //throw new System.NotImplementedException();
    }
}

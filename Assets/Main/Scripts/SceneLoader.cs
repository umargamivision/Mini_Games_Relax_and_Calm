using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string mainMenuScene;
    public GameObject loadingScreen;
    public void LoadMiniGameScene(string miniGameSceneName)
    {
        StartCoroutine(LoadSceneAsync(miniGameSceneName));
    }
    public void LoadMainMenuScene()
    {
        StartCoroutine(LoadSceneAsync(mainMenuScene));
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true); 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadingScreen.SetActive(false);  
    }
}
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
    public void LoadSettingScene()
    {
        if(SceneManager.GetSceneByName("Setting").isLoaded) return;
        var v =SceneManager.LoadSceneAsync("Setting",LoadSceneMode.Additive);
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
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public void LoadMiniGame(string _name)
    {
        GameManager.Instance.comeFromMainMenu = true;
        sceneLoader.LoadMiniGameScene(_name);
    }
}

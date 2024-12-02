using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Ommy.Audio;
using Unity.Loading;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public void LoadMiniGame(string _name)
    {
        GameManager.Instance.comeFromMainMenu = true;
        sceneLoader.LoadMiniGameScene(_name);
        AudioManager.Instance.PlaySFX(SFX.buttonClick);
    }
    public void SettingClick()
    {
        sceneLoader.LoadSettingScene();
    }
}

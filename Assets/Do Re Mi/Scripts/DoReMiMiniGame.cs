using System;
using System.Collections;
using System.Collections.Generic;
using DoReMiSpace;
using Ommy.Audio;
using UnityEngine;

public class DoReMiMiniGame : MiniGameBase
{
    public enum Mode {Tap, Voice}
    public Mode mode;
    public PlayerController playerController;
    public GameObject modeSelectionPanel,themeSelectionPanel;
    public override void LevelComplete()
    {
        base.LevelComplete();
    }
    public override void NextLevel()
    {
        UIManager.Instance.LoadMiniGameScene("Do Re Me");
    }
    public override void ReplayLevel()
    {
        throw new System.NotImplementedException();
    }
    public override void MiniGameStart()
    {
        base.MiniGameStart();
        playerController.StartGame(mode==Mode.Voice);
    }
    public void TapModeClick()
    {
        mode = Mode.Tap;
        modeSelectionPanel.SetActive(false);
        themeSelectionPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(SFX.buttonClick);
    }
    public void VoiceModeClick()
    {
        mode = Mode.Voice;
        modeSelectionPanel.SetActive(false);
        themeSelectionPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(SFX.buttonClick);
    }
    public void SetThemeClick(int index)
    {
        SetTheme(index);
        themeSelectionPanel.SetActive(false);
        MiniGameStart();
        AudioManager.Instance.PlaySFX(SFX.buttonClick);
    }
    void SetTheme(int index)
    {
        print("Theme set index "+ index);
    }
    public void SkipThemeClick()
    {
        themeSelectionPanel.SetActive(false);
        MiniGameStart();
    }
}

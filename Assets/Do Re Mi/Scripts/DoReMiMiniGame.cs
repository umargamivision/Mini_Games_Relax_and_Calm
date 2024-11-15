using System.Collections;
using System.Collections.Generic;
using DoReMiSpace;
using UnityEngine;

public class DoReMiMiniGame : MiniGameBase
{
    public enum Mode {Tap, Voice}
    public Mode mode;
    public PlayerController playerController;
    public GameObject modeSelectionPanel;
    public override void NextLevel()
    {
        throw new System.NotImplementedException();
    }
    public override void ReplayLevel()
    {
        throw new System.NotImplementedException();
    }
    public void TapModeClick()
    {
        mode = Mode.Tap;
        modeSelectionPanel.SetActive(false);
        playerController.StartGame(false);
    }
    public void VoiceModeClick()
    {
        mode = Mode.Voice;
        playerController.StartGame(true);
        modeSelectionPanel.SetActive(false);
    }
}

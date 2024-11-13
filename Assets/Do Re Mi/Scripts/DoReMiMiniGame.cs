using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoReMiMiniGame : MiniGameBase
{
    public enum Mode {Tap, Voice}
    public Mode mode;
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
    }
    public void VoiceModeClick()
    {
        mode = Mode.Voice;
    }
}

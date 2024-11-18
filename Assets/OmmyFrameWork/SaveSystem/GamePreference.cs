using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ommy.Prefs
{
    public class GamePreference
    {
        static readonly string RedLightCurrentLevelPrefs = "RedLightCurrentLevel";
        static readonly string HamsterJumpHighScorePrefs = "HamsterJumpHighScore";
        public static int RedLightCurrentLevel
        {
            get => PlayerPrefs.GetInt(RedLightCurrentLevelPrefs, 1);
            set => PlayerPrefs.SetInt(RedLightCurrentLevelPrefs, value);
        }
        public static int HamsterJumpHighScore
        {
            get => PlayerPrefs.GetInt(HamsterJumpHighScorePrefs, 0);
            set => PlayerPrefs.SetInt(HamsterJumpHighScorePrefs, value);
        }
    }
}
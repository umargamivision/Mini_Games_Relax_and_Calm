using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Ommy.Prefs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RedLightMiniGame : MiniGameBase
{
    public static RedLightMiniGame Instance;
    public void Awake()
    {
        Instance = this;
    }
    public int gameTime;
    [Header("UI")]
    public TMP_Text levelNoTxt;
    public TMP_Text gameTimerTxt;
    public UnityEvent OnGameStart;
    int currentTimer;
    public void Start()
    {
        GameStart();
    }
    public void GameStart()
    {
        OnGameStart.Invoke();
        MiniGameStart();
        gameTimerCoroutine = StartCoroutine(GameTimerCountDown(LevelFail));
        levelNoTxt.text = GamePreference.RedLightCurrentLevel.ToString();
    }
    public override void LevelComplete()
    {
        base.LevelComplete();
        GamePreference.RedLightCurrentLevel++;
    }
    public override void LevelFail()
    {
        base.LevelFail();
    }
    public override void NextLevel()
    {
        UIManager.Instance.LoadMiniGameScene("Red Light");
    }
    public override void ReplayLevel()
    {
        UIManager.Instance.LoadMiniGameScene("Red Light");
    }
    Coroutine gameTimerCoroutine;
    IEnumerator GameTimerCountDown(Action onTimerEnd)
    {
        currentTimer=gameTime;
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while(currentTimer>0)
        {
            yield return waitForSeconds;
            currentTimer--;
            gameTimerTxt.text = currentTimer.ToString("00:00");
        }
        onTimerEnd?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ommy.Prefs;
using RedLightMiniGameSpace;
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
    public Player p1,p2;
    public GameObject[] levels;
    [Header("UI")]
    public TMP_Text levelNoTxt;
    public TMP_Text gameTimerTxt;
    public UnityEvent OnGameStart;
    int currentTimer;
    public void Start()
    {
        //GameStart();
    }
    public void GameStart()
    {
        MiniGameStart();
        SetupLevel(GamePreference.RedLightCurrentLevel);
        gameTimerCoroutine = StartCoroutine(GameTimerCountDown(LevelFail));
        levelNoTxt.text = GamePreference.RedLightCurrentLevel.ToString();
        OnGameStart.Invoke();
    }
    public void SetupLevel(int levelNo)
    {
        if (levelNo > levels.Length)
        {
            GamePreference.RedLightCurrentLevel = 1;
            levelNo = 1;
        }
        levels.ToList().ForEach(f => f.SetActive(false));
        levels[levelNo - 1].SetActive(true);
    }
    public void SetupCharacter(int index = 0)
    {
        (p1 as RedLightPlayer).SetCharacter(index);
        GameStart();
    }
    public void NoThanksClick()
    {
        GameStart();
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
        currentTimer = gameTime;
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (currentTimer > 0)
        {
            yield return waitForSeconds;
            currentTimer--;
            gameTimerTxt.text = currentTimer.ToString("00:00");
        }
        onTimerEnd?.Invoke();
    }
}

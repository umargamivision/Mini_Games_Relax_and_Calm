using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.Prefs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HamsterJumpMiniGame : MiniGameBase
{
    public float resultDely;
    [Header("UI")]
    public TMP_Text hightScoreTxt;
    public TMP_Text scoreTxt;
    public UnityEvent onPlay;
    int score;
    private void Start() 
    {
        StartGame();
    }
    public void StartGame()
    {
        hightScoreTxt.text = GamePreference.HamsterJumpHighScore.ToString();
        scoreTxt.text = 0.ToString();
        MiniGameStart();
    }
    public void PlayClick()
    {
        onPlay.Invoke();
    }
    public void AddScore(int _score)
    {
        score+=_score;
        if(GamePreference.HamsterJumpHighScore<score)
        {
            GamePreference.HamsterJumpHighScore=score;
        }
        scoreTxt.text=score.ToString();
    }
    public void ShowFailResult()
    {
        StartCoroutine(DelyAction(LevelFail));
    }
    IEnumerator DelyAction(Action action)
    {
        yield return new WaitForSeconds(resultDely);
        action.Invoke();
    }
    public override void LevelFail()
    {
        base.LevelFail();
    }
    public override void NextLevel()
    {
        throw new System.NotImplementedException();
    }

    public override void ReplayLevel()
    {
        UIManager.Instance.LoadMiniGameScene("Hamster Jump");
    }
}

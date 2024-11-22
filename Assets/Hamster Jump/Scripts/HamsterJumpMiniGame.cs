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
    public GameObject arrow1, arrow2;
    public UnityEvent onPlay;
    int score;
    public void Start()
    {
        StartGame();
        StartCoroutine(Tutorial());
    }
    public IEnumerator Tutorial()
    {
        while (true)
        {
            arrow1.SetActive(true);
            arrow2.SetActive(false);
            yield return new WaitForSeconds(1);
            arrow1.SetActive(false);
            arrow2.SetActive(true);
            yield return new WaitForSeconds(1);
        }
    }
    public void StartGame()
    {
        //hightScoreTxt.text = GamePreference.HamsterJumpHighScore.ToString();
        MiniGameStart();
        hightScoreTxt.text = miniGameData.highScore.ToString();
        scoreTxt.text = 0.ToString();
    }
    public void PlayClick()
    {
        onPlay.Invoke();
    }
    public void AddScore(int _score)
    {
        score += _score;
        //if (GamePreference.HamsterJumpHighScore < score)
        if (miniGameData.highScore < score)
        {
            //GamePreference.HamsterJumpHighScore = score;
            miniGameData.highScore = score;
        }
        scoreTxt.text = score.ToString();
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
    public SpriteRenderer playerSprite;
    public Sprite[] characters;
    public void SetCharacter(int index)
    {
        playerSprite.sprite = characters[index];
    }
    public void SetCharacterAd(int index)
    {
        AdsManager.ShowRewardedAd(()=>
        {
            SetCharacter(index);
        }, "HamsterCharacter");
        //AdController.instance.ShowAd(AdController.AdType.INTERSTITIAL, "PopItLevelUnlock");
        // i want to call this line after success
        //SetLevelClick(level);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DoReMiSpace;
using Ommy.Audio;
using UnityEngine;

public class PopItMiniGame : MiniGameBase
{
    [Header("Level Selection")]
    public GameObject levelSelectionPanel;
    public int openPos, clossPos;
    [Header("Level Complete")]
    public GameObject amazing;
    public ParticleSystem completeParticle;
    public int currentLevel;
    public GameObject[] levels;
    private void Start()
    {
        StartCoroutine(DelyAction(() => ActiveLevelSelection(false), 1));
        SetLevel(currentLevel);
    }
    public override void NextLevel()
    {
        throw new System.NotImplementedException();
    }

    public override void ReplayLevel()
    {
        throw new System.NotImplementedException();
    }
    public void OnCompleteLevel()
    {
        amazing.SetActive(true);
        amazing.transform.localScale = Vector3.zero;
        amazing.transform.DOScale(1, 0.3f).OnComplete(() =>
        {

        });
        StartCoroutine(DelyAction(SetNextLevel, 2));
    }
    public IEnumerator DelyAction(Action action, float dely)
    {
        yield return new WaitForSeconds(dely);
        action.Invoke();
    }
    public void SetNextLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Length)
        {
            currentLevel = 0;
        }
        SetLevel(currentLevel);
    }
    public void ResetLevel()
    {
        amazing.SetActive(false);
    }
    public void SetLevel(int level)
    {
        ResetLevel();
        foreach (var item in levels)
        {
            item.SetActive(false);
        }
        levels[level].SetActive(true);
    }
    public void SetLevelClick(int level)
    {
        SetLevel(level);
        AudioManager.Instance.PlaySFX(SFX.buttonClick);
    }
    public void ActiveLevelSelection(bool active)
    {
        if (active)
        {
            levelSelectionPanel.transform.DOMoveX(openPos, 0.5f);
        }
        else
        {
            levelSelectionPanel.transform.DOMoveX(clossPos, 0.5f);
        }
        AudioManager.Instance.PlaySFX(SFX.buttonClick);
    }

    public void PlaySFX(AudioClip audioClip)
    {
        AudioManager.Instance.PlaySFX(audioClip);
    }
    public void SetLevelClickAd(int level)
    {
        AdsManager.ShowRewardedAd(()=>{
            SetLevelClick(level);
        }, "PopItLevelUnlock");
        //AdController.instance.ShowAd(AdController.AdType.INTERSTITIAL, "PopItLevelUnlock");
        // i want to call this line after success
        //SetLevelClick(level);
    }
}
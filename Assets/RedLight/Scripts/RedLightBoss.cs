using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ommy.Audio;
using TMPro;
using UnityEngine;

public class RedLightBoss : MonoBehaviour
{
    public enum LightColor
    {
        red,yellow,green
    }
    public LightColor lightColor;
    public int redLightTime=2,yellowLightTime=2,greenLightTime=4;
    public TMP_Text redLightTimerTxt;
    public AudioClip redClip, greenClip;
    public List<GameObject> redLightObjects;
    public List<GameObject> yellowLightObjects;
    public List<GameObject> greenLightObjects;
    int timer;
    private void Start() {
        
    }
    public void GameStart()
    {
        StartCoroutine(RedLightTimer());
    }
    public IEnumerator RedLightTimer()
    {
        GreenLight();
        while(true)
        {
            timer=greenLightTime;
            redLightTimerTxt.text = timer.ToString("00");
            while(timer>0)
            {
                yield return new WaitForSeconds(1);
                timer--;
                redLightTimerTxt.text = timer.ToString("00");
                if(timer>yellowLightTime)
                {
                    GreenLight();
                }
                else
                {
                    YellowLight();
                }
            }
            RedLight();
            yield return new WaitForSeconds(redLightTime);
            GreenLight();
            AudioManager.Instance?.PlaySFX(greenClip);
        }
    }
    void ActivateObject(LightColor lightColor, bool active)
    {
        if(lightColor==LightColor.green) 
        {
            greenLightObjects.ForEach(f=>f.SetActive(active));
        }
        else if(lightColor==LightColor.red) 
        {
            redLightObjects.ForEach(f=>f.SetActive(active));
        }
        else if(lightColor==LightColor.yellow) 
        {
            yellowLightObjects.ForEach(f=>f.SetActive(active));
        }
    }
    public void RedLight()
    {
        redLightTimerTxt.color=Color.red;
        lightColor=LightColor.red;
        ActivateObject(LightColor.green,false);
        ActivateObject(LightColor.yellow,false);
        ActivateObject(LightColor.red,true);
        AudioManager.Instance?.PlaySFX(redClip);
    }
    public void YellowLight()
    {
        redLightTimerTxt.color=Color.yellow;
        lightColor=LightColor.yellow;
        ActivateObject(LightColor.red,false);
        ActivateObject(LightColor.green,false);
        ActivateObject(LightColor.yellow,true);
    }
    public void GreenLight()
    {
        redLightTimerTxt.color=Color.green;
        lightColor=LightColor.green;
        ActivateObject(LightColor.red,false);
        ActivateObject(LightColor.yellow,false);
        ActivateObject(LightColor.green,true);
    }
}

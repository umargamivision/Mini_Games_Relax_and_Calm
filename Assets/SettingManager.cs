using System.Collections;
using System.Collections.Generic;
using EasyMobile.Demo;
using Ommy.Audio;
using Ommy.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SettingManager : MonoBehaviour
{
    public TMP_Text musicTxt,soundTxt,vibrationTxt;
    private void OnEnable() 
    {
        musicTxt.text = SaveData.Instance.Music? "On" : "Off";
        soundTxt.text = SaveData.Instance.SFX? "On" : "Off";
        vibrationTxt.text = SaveData.Instance.Haptic? "On" : "Off";
    }
    public void MusicClick()
    {
        SaveData.Instance.Music = !SaveData.Instance.Music;
        AudioManager.Instance?.SetBGSetting(SaveData.Instance.Music);
        musicTxt.text = SaveData.Instance.Music? "On" : "Off";
    }
    public void SoundClick()
    {
        SaveData.Instance.SFX = !SaveData.Instance.SFX;
        AudioManager.Instance?.SetSFXSetting(SaveData.Instance.SFX);
        soundTxt.text = SaveData.Instance.SFX? "On" : "Off";

    }
    public void VibrationClick()
    {
        SaveData.Instance.Haptic = !SaveData.Instance.Haptic;
        //AudioManager.Instance?.SetBGSetting(SaveData.Instance.Haptic);
        vibrationTxt.text = SaveData.Instance.Haptic? "On" : "Off";
    }
    public void FacebookClick()
    {

    }
    public void TiktokClick()
    {

    }
    public void ConfirmClick()
    {
        SceneManager.UnloadSceneAsync("Setting");
    }
}

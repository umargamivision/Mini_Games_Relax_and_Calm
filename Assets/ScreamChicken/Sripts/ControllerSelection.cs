using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ControllerSelection : MonoBehaviour
{
    public GameObject controller;
    // Start is called before the first frame update
    
   public void OnClickTouchController()
    {
        PlayerPrefs.SetInt("Touch", 1);
        PlayerPrefs.SetInt("Voice", 0);
        ControllerSetter.instance.SetController();
        Time.timeScale = 1;
        controller.SetActive(false);

        
        //SceneManager.LoadScene("ScreamChicken");
    }
    public void OnClickVoiceController()
    {
        PlayerPrefs.SetInt("Touch", 0);
        PlayerPrefs.SetInt("Voice", 1);
        ControllerSetter.instance.SetController();
        Time.timeScale = 1;
        controller.SetActive(false);
        //SceneManager.LoadScene("ScreamChicken");
    }
    public void OnControllerMainMenu()
    {
        PlayerPrefs.SetInt("MainMenuScene",1);
        SceneManager.LoadScene("ScreamChicken");
    }
}

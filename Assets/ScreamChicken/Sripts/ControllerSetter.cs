using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerSetter : MonoBehaviour
{
    public GameObject controllerPanel;
    public ChickenController chickenController;
    public ChickenControllerVoice chickenControllerVoice;
    // Start is called before the first frame update
    void Start()
    {
        int y = PlayerPrefs.GetInt("MainMenuScene");
        if (y == 0)
        {
            int x = PlayerPrefs.GetInt("Touch");
            if (x == 1)
            {
                chickenControllerVoice.enabled = false;
                chickenController.enabled = true;
                Debug.Log("Touch controller active start");
            }
            else
            {
                chickenController.enabled = false;
                chickenControllerVoice.enabled = true;
                Debug.Log("Voice controller active start");
            }
        }

    }
    public void SetController()
    {
        controllerPanel.SetActive(false);
        int x = PlayerPrefs.GetInt("Touch");
        if (x == 1)
        {
            chickenControllerVoice.enabled = false;
            chickenController.enabled = true;
            Debug.Log("Touch controller active");
        }
        else
        {
            chickenController.enabled = false;
            chickenControllerVoice.enabled = true;
            Debug.Log("Voice controller active");
        }
        chickenController.gameObject.SetActive(true);
    }
    public void OnClickTouchController()
    {
        PlayerPrefs.SetInt("Touch", 1);
        PlayerPrefs.SetInt("Voice", 0);
        SetController();
        Time.timeScale = 1;


        //SceneManager.LoadScene("ScreamChicken");
    }
    public void OnClickVoiceController()
    {
        PlayerPrefs.SetInt("Touch", 0);
        PlayerPrefs.SetInt("Voice", 1);
        SetController();
        Time.timeScale = 1;
        //SceneManager.LoadScene("ScreamChicken");
    }
    public void OnClickBackButton()
    {
        SceneManager.LoadScene("ScreamChickenController");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerSetter : MonoBehaviour
{

    public static ControllerSetter instance;

    private ChickenController chickenController;
    private ChickenControllerVoice chickenControllerVoice;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        chickenController = GetComponent<ChickenController>();
        chickenControllerVoice = GetComponent<ChickenControllerVoice>();
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
    }

    public void OnClickBackButton()
    {
        SceneManager.LoadScene("ScreamChickenController");
    }
}

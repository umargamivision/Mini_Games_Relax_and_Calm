using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutInHalf_MiniGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("CutInHalf");
    }

   
}

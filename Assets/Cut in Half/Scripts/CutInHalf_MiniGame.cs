using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutInHalf_MiniGame : MiniGameBase
{
    public List<GameObject> levelObjects;
    public Transform targetPositionObject;
    public static CutInHalf_MiniGame instance;
    // Start is called before the first frame update
    void Start()
    {
        int levelno = PlayerPrefs.GetInt("CutInHalf_LevelNo");
        Debug.Log("Level no is " + levelno);

        if (levelno >= 2)
        {
            levelno = 0;
            PlayerPrefs.SetInt("CutInHalf_LevelNo", 0);
        }

        LoadLevel(levelno);
        
        instance = this;
        
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("CutInHalf");
    }
    public void CustomLevelComplete()
    {
        // increment player pref 
    }
    public override void LevelComplete()
    {
        base.LevelComplete();
        print("level complete");
        UIManager.Instance.ShowLevelComplete();
    }
    public override void NextLevel()
    {
        // restart this scene or reset items values
        UIManager.Instance.LoadMiniGameScene("CutInHalf");
       // throw new System.NotImplementedException();
    }

    public override void ReplayLevel()
    {
        UIManager.Instance.LoadMiniGameScene("CutInHalf");
       // throw new System.NotImplementedException();
    }
    public void LoadLevel(int x)
    {
        if (targetPositionObject.childCount > 0)
        {
            foreach (Transform child in targetPositionObject)
            {
                Destroy(child.gameObject);
            }
        }

        // Instantiate the prefab as a child of the target position object
        GameObject newInstance = Instantiate(levelObjects[x], targetPositionObject.position, Quaternion.identity, targetPositionObject);
        newInstance.transform.localPosition = Vector3.zero; // Optional: reset local position to align within the parent

    }
}

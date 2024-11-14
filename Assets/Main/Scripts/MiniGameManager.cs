using Unity.VisualScripting;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    private void Start() {
        Application.targetFrameRate = 30;
    }
    public virtual void MiniGameStart()
    {
        Debug.Log("GAME START");
    }
    public virtual void LevelComplete()
    {
        Debug.Log("GAME COMPLETE");
        UIManager.Instance.ShowLevelComplete();
    }
    public virtual void LevelFail()
    {
        Debug.Log("GAME FAIL");
        UIManager.Instance.ShowLevelFail();
    }
    public abstract void NextLevel();
    public abstract void ReplayLevel();
}
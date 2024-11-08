using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterLevelSpawner : MonoBehaviour
{
    public int playerDistanceSpawnLevel;
    public Transform player;
    public int initalSpawn;
    public Transform startPoint;
    public List<GameObject> levelParts;
    Transform lastEndPoint;

    private void Awake() 
    {
        for (int i = 0; i < initalSpawn; i++)
        {
            SpawnLevel();
        }    
    }
    private void Update() 
    {
        if(Vector3.Distance(player.position, lastEndPoint.position)<playerDistanceSpawnLevel)
        {
            SpawnLevel();
        }
    }
    public void SpawnLevel()
    {
        var levelPart = Instantiate(levelParts[Random.Range(0, levelParts.Count)], lastEndPoint? lastEndPoint.position : startPoint.position,Quaternion.identity);
        lastEndPoint = levelPart.transform.Find("EndPoint");
    }
}

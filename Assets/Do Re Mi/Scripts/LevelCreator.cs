using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoReMiSpace
{
    public class LevelCreator : MonoBehaviour
    {
        public List<PlatformDoReMe> platforms;
        public PlatformDoReMe currentPlatform => platforms[0];
        public GameObject[] tilePrefabs;
        public Vector3 startPoint;
        public int numberOfTiles;
        public float distanceBetweenTiles;
        private void Start() 
        {
            CreateLevel();
        }
        public void CreateLevel()
        {
            for (int i = 0; i < numberOfTiles; i++)
            {
                GameObject platformObj = Instantiate(tilePrefabs[Random.Range(0,tilePrefabs.Length)], startPoint+Vector3.forward*(distanceBetweenTiles*i) ,Quaternion.identity);
                var platform = platformObj.GetComponent<PlatformDoReMe>();
                platform.onPlayerPassed.AddListener(OnPlayerPassed);
                platforms.Add(platform);
            }
        }
        public void OnPlayerPassed()
        {
            platforms.Remove(currentPlatform);
        }
    }
}

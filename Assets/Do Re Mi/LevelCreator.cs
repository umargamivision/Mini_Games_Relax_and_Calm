using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoReMiSpace
{
    public class LevelCreator : MonoBehaviour
    {
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
                Instantiate(tilePrefabs[Random.Range(0,tilePrefabs.Length)], startPoint+Vector3.forward*(distanceBetweenTiles*i) ,Quaternion.identity);
            }
        }
    }
}

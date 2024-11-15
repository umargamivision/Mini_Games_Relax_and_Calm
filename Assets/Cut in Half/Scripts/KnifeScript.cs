using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    public KnifeSliceManager sliceManager; // Reference to the KnifeSliceManager script

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with has a "Sliceable" tag (or other identifier)
        if (collision.gameObject.CompareTag("Sliceable"))
        {
            // Get the collision point (the contact point between the knife and the sliceable object)
            Vector3 collisionPoint = collision.contacts[0].point;

            // Call the SliceSprite function on the KnifeSliceManager, passing in the sliceable object and the collision point
            sliceManager.SliceSprite(collision.gameObject, collisionPoint);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (KnifeSliceManager.instance.weightPercentage == 50)
            {
                KnifeSliceManager.instance.win = true;
                int levelno = PlayerPrefs.GetInt("CutInHalf_LevelNo");
                levelno = levelno + 1;
                Debug.Log("level no is " + levelno + " AFTER INCREMENT");
                if (levelno >= 2)
                {
                    levelno = 0;
                }
                PlayerPrefs.SetInt("CutInHalf_LevelNo", levelno);

            }

          
            KnifeSliceManager.instance.CheckLevelCompletetion();
        }
    }
}

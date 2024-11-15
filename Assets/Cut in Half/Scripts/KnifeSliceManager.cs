using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KnifeSliceManager : MonoBehaviour
{
    public static KnifeSliceManager instance;
    public int weightPercentage;
    public bool win;
    public GameObject knife;
    public Transform targetPosition1; // Target position for the first sliced part
    public Transform targetPosition2; // Target position for the second sliced part
    public float moveSpeed = 2.0f; // Speed of movement towards target
    public float rotationSpeed = 100.0f; // Speed of rotation once at target
    public Text textBoxForFragment1; // Text box for displaying the weight of the first fragment
    public Text textBoxForFragment2; // Text box for displaying the weight of the second fragment
    public float textAnimationSpeed = 1.0f; // Speed for the text animation

    private bool hasSliced = false;

    private void Start()
    {
        instance = this;
        win = false;
    }
    // Call this method to perform slicing, e.g., when the knife collides with the object
    public void SliceSprite(GameObject objectToSlice, Vector3 collisionPoint)
    {
        if (hasSliced) return; // Prevent multiple slicing

        hasSliced = true;

        // Get the original sprite's area using SpriteRenderer
        SpriteRenderer originalSpriteRenderer = objectToSlice.GetComponent<SpriteRenderer>();
        if (originalSpriteRenderer == null)
        {
            Debug.LogWarning("The object to slice does not have a SpriteRenderer component.");
            return;
        }

        int originalArea = Mathf.RoundToInt(originalSpriteRenderer.bounds.size.x * originalSpriteRenderer.bounds.size.y);

        // List to store information about the sliced fragments
        List<SpriteSlicer2DSliceInfo> slicedSpriteInfo = new List<SpriteSlicer2DSliceInfo>();
        Vector3 endPoint = new Vector3(collisionPoint.x, collisionPoint.y - 10.0f, collisionPoint.z);

        // Perform the slicing operation
        SpriteSlicer2D.SliceAllSprites(collisionPoint, endPoint, true, ref slicedSpriteInfo);

        // Move fragments to target positions, calculate their areas, and display weights
        int fragmentIndex = 0;
        int firstFragmentWeight = 0;

        for (int i = 0; i < slicedSpriteInfo.Count; i++)
        {
            foreach (var fragment in slicedSpriteInfo[i].ChildObjects)
            {
                MeshRenderer fragmentRenderer = fragment.GetComponent<MeshRenderer>();
                if (fragmentRenderer != null)
                {
                    if (fragmentIndex == 0) // Calculate weight only for the first fragment
                    {
                        int fragmentArea = Mathf.RoundToInt(fragmentRenderer.bounds.size.x * fragmentRenderer.bounds.size.y);
                        firstFragmentWeight = Mathf.RoundToInt((float)fragmentArea / originalArea * 100);
                        Debug.Log("Fragment 1 occupies " + firstFragmentWeight + "% of the original object's area.");
                    }

                    int secondFragmentWeight = 100 - firstFragmentWeight; // Calculate weight of the second fragment

                    // Determine target position and rotation angle for each fragment
                    Transform targetPosition = (fragmentIndex == 0) ? targetPosition1 : targetPosition2;
                    float targetRotation = (fragmentIndex == 0) ? -90f : 90f;
                    weightPercentage = (fragmentIndex == 0) ? firstFragmentWeight : secondFragmentWeight;

                    if (targetPosition != null)
                    {
                        // Start moving the fragment to the target position with rotation
                        StartCoroutine(MoveAndRotateFragment(fragment, targetPosition.position, targetRotation));
                    }
                    else
                    {
                        Debug.LogError("Target position not assigned for fragment index: " + fragmentIndex);
                    }

                    // Animate the weight display in the corresponding text box
                    Text targetTextBox = (fragmentIndex == 0) ? textBoxForFragment1 : textBoxForFragment2;
                   
                    StartCoroutine(AnimateText(targetTextBox, weightPercentage));

                    fragmentIndex = (fragmentIndex + 1) % 2; // Alternate between target positions
                }
                else
                {
                    Debug.LogWarning("Fragment " + (i + 1) + " does not have a MeshRenderer component.");
                }
            }
        }

        hasSliced = false; // Reset the slicing flag if needed for future slicing operations
    }


    IEnumerator MoveAndRotateFragment(GameObject fragment, Vector3 targetPosition, float targetRotation)
    {
        // Move the fragment towards the target position at a constant speed
        while (Vector3.Distance(fragment.transform.position, targetPosition) > 0.01f)
        {
            fragment.transform.position = Vector3.MoveTowards(fragment.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Rotate the fragment to the specified angle over time
        Quaternion endRotation = Quaternion.Euler(0, 0, targetRotation);
        while (Quaternion.Angle(fragment.transform.rotation, endRotation) > 0.5f)
        {
            fragment.transform.rotation = Quaternion.RotateTowards(fragment.transform.rotation, endRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        fragment.transform.rotation = endRotation; // Ensure final rotation is exact
        Debug.Log("Rotated fragment to " + targetRotation + " degrees.");
    }
    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(3f);
        CutInHalf_MiniGame.instance.LevelComplete();
    }
    IEnumerator LevelFail()
    {
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ShowLevelFail();
    }
   IEnumerator AnimateText(Text targetTextBox, int targetValue)
    {
        int currentValue = 0;
        while (currentValue < targetValue)
        {
            currentValue += Mathf.CeilToInt(textAnimationSpeed); // Increment by the animation speed
            if (currentValue > targetValue) currentValue = targetValue; // Ensure it doesn’t go over the target value
            targetTextBox.text = currentValue + "%";
            yield return new WaitForSeconds(0.03f); // Control the update speed for a smooth animation effect
        }
    }
    public void CheckLevelCompletetion()
    {
        if (win)
        {
            StartCoroutine(LevelComplete());
        }
        else
        {
            StartCoroutine(LevelFail());
        }
    }
}

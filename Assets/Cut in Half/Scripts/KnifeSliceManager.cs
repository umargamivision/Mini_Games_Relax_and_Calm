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
    public Transform targetPosition1; // Target position for the right sliced part
    public Transform targetPosition2; // Target position for the left sliced part
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

        // Ensure we have exactly two fragments
        if (slicedSpriteInfo.Count == 1 && slicedSpriteInfo[0].ChildObjects.Count == 2)
        {
            var child1 = slicedSpriteInfo[0].ChildObjects[0];
            var child2 = slicedSpriteInfo[0].ChildObjects[1];

            // Use bounds to determine which fragment is on the right
            Renderer bounds1 = child1.GetComponent<Renderer>();
            Renderer bounds2 = child2.GetComponent<Renderer>();

            if (bounds1 == null || bounds2 == null)
            {
                Debug.LogError("Sliced fragments are missing Renderer components.");
                return;
            }

            // Compare bounds to determine right and left fragments
            GameObject rightFragment = bounds1.bounds.center.x > bounds2.bounds.center.x ? child1 : child2;
            GameObject leftFragment = rightFragment == child1 ? child2 : child1;

            // Assign target positions and rotations
            StartCoroutine(MoveAndRotateFragment(rightFragment, targetPosition1.position, -90f));
            StartCoroutine(MoveAndRotateFragment(leftFragment, targetPosition2.position, 90f));

            // Calculate and display weights for the fragments
            AssignWeightsAndAnimateText(rightFragment, leftFragment, textBoxForFragment1, textBoxForFragment2, originalArea);
        }
        else
        {
            Debug.LogError("Slicing failed or resulted in an unexpected number of fragments.");
        }

        hasSliced = false; // Reset the slicing flag if needed for future slicing operations
    }

    private void AssignWeightsAndAnimateText(GameObject fragment1, GameObject fragment2, Text textBox1, Text textBox2, int originalArea)
    {
        // Calculate the area of the first fragment
        MeshRenderer renderer1 = fragment1.GetComponent<MeshRenderer>();
        if (renderer1 != null)
        {
            int fragment1Area = Mathf.RoundToInt(renderer1.bounds.size.x * renderer1.bounds.size.y);
            int fragment1Weight = Mathf.RoundToInt((float)fragment1Area / originalArea * 100);

            // Assign the weight of the first fragment
            weightPercentage = fragment1Weight;

            // Automatically calculate the weight of the second fragment
            int fragment2Weight = 100 - fragment1Weight;

            // Animate the weight display for both text boxes
            StartCoroutine(AnimateText(textBox1, fragment1Weight));
            StartCoroutine(AnimateText(textBox2, fragment2Weight));
        }
        else
        {
            Debug.LogWarning("First fragment does not have a MeshRenderer component.");
        }
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
        Debug.Log("Fragment moved and rotated to target position.");
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

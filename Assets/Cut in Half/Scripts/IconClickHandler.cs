using UnityEngine;
using UnityEngine.UI;

public class IconClickHandler : MonoBehaviour
{
    public GameObject prefabToInstantiate; // Prefab to be instantiated on click
    public Transform targetPositionObject; // Reference to the Transform where the prefab will be instantiated

    private void Start()
    {
        // Ensure the prefab and target position object are assigned in the Inspector
        if (prefabToInstantiate == null)
        {
            Debug.LogWarning("PrefabToInstantiate is not assigned in the Inspector.");
        }

        if (targetPositionObject == null)
        {
            Debug.LogWarning("TargetPositionObject is not assigned in the Inspector.");
        }

        // Get the Button component and add an onClick listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnIconClick);
        }
    }

    // This method is called when the icon is clicked
    public void OnIconClick()
    {
        if (prefabToInstantiate != null && targetPositionObject != null)
        {
            // Check if there's already a child in the target position and delete it
            if (targetPositionObject.childCount > 0)
            {
                foreach (Transform child in targetPositionObject)
                {
                    Destroy(child.gameObject);
                }
            }

            // Instantiate the prefab as a child of the target position object
            GameObject newInstance = Instantiate(prefabToInstantiate, targetPositionObject.position, Quaternion.identity, targetPositionObject);
            newInstance.transform.localPosition = Vector3.zero; // Optional: reset local position to align within the parent
        }
        else
        {
            Debug.LogWarning("Either PrefabToInstantiate or TargetPositionObject is not set.");
        }
    }
}

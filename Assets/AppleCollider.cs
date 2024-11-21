using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCollider : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Get the PolygonCollider2D component attached to this GameObject
        polygonCollider = GetComponent<PolygonCollider2D>();

        if (polygonCollider != null)
        {
            // Simplify the collider with a specified tolerance
            SimplifyCollider(polygonCollider, 0.1f); // Adjust tolerance as needed
        }
        else
        {
            Debug.LogError("No PolygonCollider2D component found on this GameObject!");
        }
    }

    void SimplifyCollider(PolygonCollider2D collider, float tolerance = 0.1f)
    {
        Vector2[] points = collider.GetPath(0); // Get the current collider path
        List<Vector2> simplifiedPoints = new List<Vector2>();

        for (int i = 0; i < points.Length; i++)
        {
            // Only add points that differ significantly from the last one
            if (i == 0 || Vector2.Distance(points[i], points[i - 1]) > tolerance)
            {
                simplifiedPoints.Add(points[i]);
            }
        }

        collider.SetPath(0, simplifiedPoints.ToArray());
        Debug.Log("Collider simplified with " + simplifiedPoints.Count + " points.");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorControl : MonoBehaviour
{
    public GameObject indicatorPrefab;
    private GameObject currentIndicator;
    public void ShowIndicator(Vector3 position, float sprayRadius)
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        Debug.Log($"Showing indicator at position: {position}, Radius: {sprayRadius}");

        // Adjust the y position slightly above the ground or object to avoid overlapping
        float heightOffset = 0.1f; // Slight offset above the ground or object surface
        Vector3 adjustedPosition = new Vector3(position.x, position.y + heightOffset, position.z);

        currentIndicator = Instantiate(indicatorPrefab, adjustedPosition, Quaternion.Euler(0, 0, 0));
        float diameter = sprayRadius * 2;
    }

    public void HideIndicator(float delay = 0f)
    {
        if (currentIndicator != null)
        {
            if (delay > 0f)
            {
                Destroy(currentIndicator, delay); // Destroy with delay
            }
            else
            {
                Destroy(currentIndicator); // Immediate destroy
            }
        }
    }

}
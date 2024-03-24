using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorControl : MonoBehaviour
{
    public GameObject indicatorPrefab;
    private GameObject currentIndicator;
    public float indicatorSize = 5f; // Default size

    public void ShowIndicator(Vector3 position, float size)
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator); // Remove existing indicator
        }

        currentIndicator = Instantiate(indicatorPrefab, position, Quaternion.Euler(90, 0, 0));
        currentIndicator.transform.localScale = new Vector3(size, size, 1);
    }

    public void HideIndicator()
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }
    }
}
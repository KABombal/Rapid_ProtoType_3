using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScaler : MonoBehaviour
{
    public Transform playerTransform;
    public Transform sprayCanTransform;
    public float optimalHeight = 15f; // Height for maximum shadow scale
    public float maximumHeight = 30f; // Height for minimum shadow scale
    public Vector3 maxShadowScale = new Vector3(5f, 5f, 5f); // Maximum scale of the shadow
    public Vector3 minShadowScale = new Vector3(0.2f, 0.2f, 0.2f); // Minimum scale of the shadow

    void Update()
    {
        if (playerTransform != null && sprayCanTransform != null)
        {
            float currentHeight = Mathf.Abs(sprayCanTransform.position.y - playerTransform.position.y);

            // Scale factor adjusts based on the current height in relation to optimal and maximum heights
            float scaleFactor = 0f;
            if (currentHeight <= optimalHeight)
            {
                scaleFactor = 1f;
            }
            else if (currentHeight <= maximumHeight)
            {
                // Non-linear scale factor: (1 - normalizedHeight)^2 for more dramatic effect
                float normalizedHeight = (currentHeight - optimalHeight) / (maximumHeight - optimalHeight);
                scaleFactor = 1f - Mathf.Pow(normalizedHeight, 2);
            }
            // Otherwise, scaleFactor remains 0 (at or above maximumHeight)

            // Lerp between min and max scales based on scaleFactor
            transform.localScale = Vector3.Lerp(minShadowScale, maxShadowScale, scaleFactor);
        }
    }
}

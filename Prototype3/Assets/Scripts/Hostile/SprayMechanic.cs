using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayMechanic : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject sprayCanPrefab; // Prefab for the spray can
    public GameObject sprayEffectPrefab;
    public IndicatorControl indicatorControl;

    // Initial interval timing
    public float initialMinInterval = 10f;
    public float initialMaxInterval = 15f;
    // Final interval timing
    public float finalMinInterval = 5f;
    public float finalMaxInterval = 7f;
    // Time over which the interval changes
    public float timeToDecrease = 300f; // 5 minutes

    // Movement and spraying
    public float movementDuration = 3f; // Time to move from start to spray height
    public float sprayHeight = 15f;
    public float startHeight = 30f;
    public float sprayRadius = 3f;

    private float currentMinInterval;
    private float currentMaxInterval;
    private float nextSprayTime;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        currentMinInterval = initialMinInterval;
        currentMaxInterval = initialMaxInterval;
        ScheduleNextSpray();
    }

    void Update()
    {
        if (Time.time >= nextSprayTime)
        {
            StartCoroutine(HandleSprayCycle());
            ScheduleNextSpray();
        }

        AdjustIntervalOverTime();
    }

    void ScheduleNextSpray()
    {
        nextSprayTime = Time.time + Random.Range(currentMinInterval, currentMaxInterval);
    }

    void AdjustIntervalOverTime()
    {
        float elapsedTime = Time.time - startTime;
        float progress = Mathf.Clamp01(elapsedTime / timeToDecrease);

        currentMinInterval = Mathf.Lerp(initialMinInterval, finalMinInterval, progress);
        currentMaxInterval = Mathf.Lerp(initialMaxInterval, finalMaxInterval, progress);
    }
    IEnumerator HandleSprayCycle()
    {
        // Calculate a random position within a radius around the player, at startHeight
        Vector3 randomOffset = Random.insideUnitSphere * sprayRadius;
        randomOffset.y = 0; // Only randomize x and z

        Vector3 groundPosition = new Vector3(
            playerTransform.position.x + randomOffset.x,
            playerTransform.position.y, // Player's ground level
            playerTransform.position.z + randomOffset.z
        );

        Vector3 startPosition = new Vector3(
            groundPosition.x,
            groundPosition.y + startHeight, // Start above the player's current y level
            groundPosition.z
        );

        Vector3 sprayPosition = new Vector3(
            groundPosition.x,
            groundPosition.y + sprayHeight, // Spray height above the player's current y level
            groundPosition.z
        );

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject sprayCan = Instantiate(sprayCanPrefab, startPosition, rotation);

        // Show the indicator at the target position with the same radius as the spray
        indicatorControl.ShowIndicator(groundPosition, sprayRadius);

        // Move to sprayHeight
        yield return StartCoroutine(MoveSprayCan(sprayCan, startPosition, sprayHeight + playerTransform.position.y, movementDuration));

        // Wait for a second before triggering the spray effect
        yield return new WaitForSeconds(1f);

        // Spawn the particle system at the spray head location
        Transform sprayHeadTransform = sprayCan.transform.Find("SprayHead"); // Replace "SprayHead" with the actual name
        GameObject sprayEffectInstance = null;
        if (sprayHeadTransform != null)
        {
            sprayEffectInstance = Instantiate(sprayEffectPrefab, sprayHeadTransform.position, sprayHeadTransform.rotation, sprayHeadTransform);
        }

        // Activate the particle system
        if (sprayEffectInstance != null)
        {
            Spray sprayScript = sprayEffectInstance.GetComponent<Spray>();
            if (sprayScript != null)
            {
                sprayScript.ActivateSpray();
            }
        }

        // Destroy the spray can after a delay to allow the particle effect to complete
        float particleEffectDuration = 1f; // Adjust this to the duration of your particle effect
        Destroy(sprayCan, particleEffectDuration);

        // Hide the indicator
        indicatorControl.HideIndicator();
    }

    IEnumerator MoveSprayCan(GameObject sprayCan, Vector3 start, float targetHeight, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float newY = Mathf.Lerp(start.y, targetHeight, elapsed / duration);
            sprayCan.transform.position = new Vector3(start.x, newY, start.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        sprayCan.transform.position = new Vector3(start.x, targetHeight, start.z);
    }
}
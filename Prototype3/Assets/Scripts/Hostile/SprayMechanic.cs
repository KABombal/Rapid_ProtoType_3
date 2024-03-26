using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayMechanic : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject sprayCanPrefab; // Prefab for the spray can
    public GameObject sprayEffectPrefab;
    public IndicatorControl indicatorControl;
    public DecalManager decalManager;

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
    private bool isSprayingEnabled = true; // Flag to control the spraying logic

    public Vector3 groundPosition;
    public Vector3 startPosition;
    void Start()
    {
        startTime = Time.time;
        currentMinInterval = initialMinInterval;
        currentMaxInterval = initialMaxInterval;
        ScheduleNextSpray();

        if (!decalManager)
        {
            Debug.LogError("DecalManager not assigned in SprayMechanic.");
        }
    }

    void Update()
    {
        if (!isSprayingEnabled) return;

        UpdateSprayCanPosition();

        if (Time.time >= nextSprayTime)
        {
            StartCoroutine(HandleSprayCycle());
            ScheduleNextSpray();
        }

        AdjustIntervalOverTime();

        groundPosition = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y, // Player's ground level
            playerTransform.position.z
        );

        // Spray Can spawns directly above the player
        startPosition = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + startHeight,
            playerTransform.position.z
        );
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
        // Define a layer mask that includes all layers except the Player layer
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject sprayCan = Instantiate(sprayCanPrefab, startPosition, rotation);

        // Move to sprayHeight
        yield return StartCoroutine(MoveSprayCan(sprayCan, startPosition, sprayHeight + playerTransform.position.y, movementDuration));

        // Wait for a second before triggering the spray effect
        yield return new WaitForSeconds(0.5f);

        // Spawn the particle system at the spray head location
        Transform sprayHeadTransform = sprayCan.transform.Find("SprayHead");
        GameObject sprayEffectInstance = null;
        if (sprayHeadTransform != null)
        {
            sprayEffectInstance = Instantiate(sprayEffectPrefab, sprayHeadTransform.position, sprayHeadTransform.rotation, sprayHeadTransform);

            // Cast a ray downward from the spray head to find the ground
            if (Physics.Raycast(sprayHeadTransform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Vector3 indicatorPosition = hit.point; // Position where the ray hit the ground

                // Show the indicator at this position with the same radius as the spray
                indicatorControl.ShowIndicator(indicatorPosition, sprayRadius);
            }
        }

        // Destroy the spray can after a delay to allow the particle effect to complete
        float particleEffectDuration = 1.0f; // Adjust this to the duration of your particle effect

        // Activate the particle system
        if (sprayEffectInstance != null)
        {
            Spray sprayScript = sprayEffectInstance.GetComponent<Spray>();
            if (sprayScript != null)
            {
                sprayScript.ActivateSpray();
            }

            // Wait for the particle effect to complete before creating a decal
            yield return new WaitForSeconds(1.5f);

            // Raycast down from the spray head to place the decal
            if (Physics.Raycast(sprayHeadTransform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                decalManager.CreateDecal(hit.point); // Create the decal at the raycast hit point
            }
        }

        Destroy(sprayCan, particleEffectDuration);

        // Hide the indicator
        indicatorControl.HideIndicator(particleEffectDuration);
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

    void UpdateSprayCanPosition()
    {
        // Update startPosition to be above the player, considering the player's current orientation
        startPosition = playerTransform.position + (playerTransform.up * startHeight);
    }

    public void ToggleSpraying(bool enabled)
    {
        isSprayingEnabled = enabled;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayMechanic : MonoBehaviour
{
    public Transform playerTransform;
    public float initialMinInterval = 10f;
    public float initialMaxInterval = 15f;
    public float finalMinInterval = 5f;
    public float finalMaxInterval = 7f;
    public float timeToDecrease = 300f; // 5 minutes

    private float currentMinInterval;
    private float currentMaxInterval;
    private float nextSprayTime;
    private float startTime;

    public float sprayRadius = 5f; // Radius around the player to target
    public float sprayAreaEffect = 3f; // Area of effect of the spray
    public float sprayStartHeight = 15f; // Height from which the spray starts
    public GameObject sprayEffectPrefab; // Assign this in the Unity inspector

    void Start()
    {
        currentMinInterval = initialMinInterval;
        currentMaxInterval = initialMaxInterval;
        startTime = Time.time;
        ScheduleNextSpray();
    }

    void Update()
    {
        if (Time.time >= nextSprayTime)
        {
            TriggerSpray();
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

    void TriggerSpray()
    {
        Vector3 randomDirection = Random.insideUnitSphere * sprayRadius;
        randomDirection += playerTransform.position;
        randomDirection.y = sprayStartHeight; // Set the y-coordinate to the starting height

        Vector3 sprayLocation = randomDirection;

        CreateSprayEffect(sprayLocation);
    }

    void CreateSprayEffect(Vector3 location)
    {
        // Instantiate the particle system at the location
        Instantiate(sprayEffectPrefab, location, Quaternion.identity);
    }
}

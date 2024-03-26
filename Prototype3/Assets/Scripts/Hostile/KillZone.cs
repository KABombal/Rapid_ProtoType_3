using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform checkpoint;
    private bool playerKilled = false;  // Flag to track if player was killed
    public GameObject swatterPrefab;  // Reference to the swatter prefab

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerKilled)
        {
            playerKilled = true;

            // Instantiate and activate the swatter
            GameObject swatterInstance = Instantiate(swatterPrefab, CalculateSwatterSpawnPosition(), Quaternion.identity);
            swatterInstance.GetComponent<Swatter>().ActivateSwatter(other.transform);

            Debug.Log("Player entered the kill zone. Swatter activated.");
        }
    }

    // Calculate an appropriate position to spawn the swatter
    private Vector3 CalculateSwatterSpawnPosition()
    {
        // Modify this to set an appropriate position based on the kill zone and level design
        return new Vector3(0, 10, 0); // Example: Above the kill zone
    }
}

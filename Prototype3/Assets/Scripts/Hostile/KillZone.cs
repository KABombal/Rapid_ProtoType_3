using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform checkpoint;
    private bool playerKilled = false;  // Flag to track if player was killed

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerKilled)
        {
            playerKilled = true;
            other.GetComponent<SpiderController>().LoseLife();
            // Teleport the player to the checkpoint
            Debug.Log("Player has left the playable area. Respawning at checkpoint.");
            other.transform.position = checkpoint.position;
            // Here you can also trigger any effects associated with respawning
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningZone : MonoBehaviour
{
    private bool playerEntered = false; // Flag to track if the player has entered

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerEntered) // Check for first interaction
        {
            playerEntered = true;
            // Display a warning message to the player
            Debug.Log("Warning: Approaching the boundary of the playable area!");
            // Trigger any visual/audio warning effects here
        }
    }
}
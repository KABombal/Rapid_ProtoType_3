using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    private SpiderController spiderController; // Reference to the player controller script

    void Start()
    {
        // Find the player with the "Player" tag and get the SpiderController component
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            spiderController = playerObject.GetComponent<SpiderController>();
            if (spiderController == null)
            {
                Debug.LogError("SpiderController script not found on Player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Particle collided with player.");
            // Call the function to handle player's collision with particles
            spiderController.HandleParticleCollision();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    public TestPlayer spiderController; // Reference to the player controller script

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the function to handle player's collision with particles
            spiderController.HandleParticleCollision();
        }
    }
}

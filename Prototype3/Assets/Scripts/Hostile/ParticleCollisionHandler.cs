using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    public SpiderController spiderController; // Direct reference to the player controller script

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Particle collided with player.");
            // Call the function to handle player's collision with particles
            if (spiderController != null)
            {
                spiderController.HandleParticleCollision();
            }
            else
            {
                Debug.LogError("SpiderController reference not set on ParticleCollisionHandler.");
            }
        }
    }
}

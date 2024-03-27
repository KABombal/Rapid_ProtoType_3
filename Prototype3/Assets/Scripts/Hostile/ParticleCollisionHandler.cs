using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    public SpiderController spiderController; // Direct reference to the player controller script
    private ParticleSystem partSystem; // The Particle System that will detect collisions
    private List<ParticleCollisionEvent> collisionEvents; // List to store collision events

    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(partSystem, other, collisionEvents);
        Debug.Log("Particle hit by: " + other.name);
        if (numCollisionEvents > 0)
        {
            // Handle the collision with the first particle collision event, for example
            ParticleCollisionEvent collisionEvent = collisionEvents[0];

            // Check if the particle collided with the player
            if (collisionEvent.colliderComponent.CompareTag("Player"))
            {
                Debug.Log("Particle collided with player.");
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
}

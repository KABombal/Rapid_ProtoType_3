using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Decal hit: " + other.name);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SpiderController>().HandleParticleCollision();
        }
    }
}

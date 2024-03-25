using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the player script has a method called 'LoseLife'
            other.GetComponent<SpiderController>().LoseLife();
        }
    }
}

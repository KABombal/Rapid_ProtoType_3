using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    public ParticleSystem sprayEffect; 

    public void ActivateSpray()
    {
        Debug.Log("Spray activated");
        // Activate the spray particle effect
        sprayEffect.Play();
    }
}

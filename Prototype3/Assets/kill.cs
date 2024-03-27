using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kill : MonoBehaviour
    
{
    public SpiderController spiderController; // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (spiderController != null)
        {
            spiderController.LoseLife();
        }
        else
        {
            Debug.LogError("SpiderController reference not set on Swatter.");
        }
    }
}

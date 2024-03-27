using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint_Scr : MonoBehaviour
{
    [SerializeField] private int id = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.name != "Spider")
        {
            Debug.Log(other.gameObject.name);
            return;
        }
        GameManager_Scr.Instance.CheckpointHit(id,gameObject);
        
    }
}

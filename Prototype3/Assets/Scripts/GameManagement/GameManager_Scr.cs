using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Scr : MonoBehaviour
{

    public static GameManager_Scr Instance { get; private set; }
    public GameObject UI_Manager;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            Player = GameObject.Find("Spider");
            Debug.Log("GameManager Started");
        }
    }

    [SerializeField] private GameObject RespawnPoint;
    private GameObject Player;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckpointHit(int _id, GameObject _Checkpoint)
    {
        if(_id == 1)
        {
            ChangeRespawnPoint(_Checkpoint);
        }
    }


    [ContextMenu("Kill Player")]
    public void OnPlayerDeath()
    {
        if (Player != null)
        {
            
            Player.transform.position = RespawnPoint.transform.position;
           
            Debug.Log("Player respawned");
        }
        else
        {
            Debug.Log("Player Not Set");
        }
    }

    private void ChangeRespawnPoint(GameObject _Checkpoint)
    {
        if(_Checkpoint == null) { return; }
        RespawnPoint = _Checkpoint;
    }
}

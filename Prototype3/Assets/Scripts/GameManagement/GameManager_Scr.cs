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
        //if(_id == 1)
        //{
        //    ChangeRespawnPoint(_Checkpoint);
        //}
        switch(_id)
        {
            case 0:     // None
                Debug.Log("id not set for checkpoint");
                break;
                case 1: // RespawnPoint
                ChangeRespawnPoint(_Checkpoint);
                break;
            case 2:     // DamageField
                Player.GetComponent<SpiderController>().HandleParticleCollision();
                break;
            case 3:     // Fly
                Player.GetComponent<SpiderController>().AddScore(1);
                break;
            case 4:
                if (Player.GetComponent<SpiderController>().Flys_Score > 6)
                {
                    Debug.Log("You win");//Add cutscene here
                }
                else
                {
                    Debug.Log("Not enough fly's. Your children will starve");
                }
                break;
            default:

                break;
        }
    }


    [ContextMenu("Kill Player")]
    public void OnPlayerDeath()
    {
        if (Player != null)
        {
            Player.GetComponent<SpiderController>().isActive = false;
            Player.GetComponent<SpiderController>().Warp(RespawnPoint.transform.position);
            Player.GetComponent<SpiderController>().isActive = true;

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

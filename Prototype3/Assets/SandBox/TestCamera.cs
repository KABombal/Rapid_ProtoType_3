using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0, 10, -7);

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
            transform.LookAt(playerTransform);
        }
    }
}

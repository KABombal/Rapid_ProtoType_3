using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour
{
    public GameObject decalPrefab; // Assign in inspector
    public float decalLifetime = 60f;

    public void CreateDecal(Vector3 position)
    {
        GameObject decal = Instantiate(decalPrefab, position, Quaternion.identity);
        decal.transform.localScale = new Vector3(10f, 1f, 10f); // Set the size of the decal
        Destroy(decal, decalLifetime);
    }
}
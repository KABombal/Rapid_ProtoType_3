using UnityEngine;

public class DecalManager : MonoBehaviour
{
    public GameObject decalPrefab; // Assign your decal prefab in the Inspector

    // Function to create a decal at a given position
    public void CreateDecal(Vector3 position)
    {
        if (decalPrefab != null)
        {
            GameObject decal = Instantiate(decalPrefab, position, Quaternion.Euler(90, 0, 0)); // Adjust rotation if needed
            // You can add additional logic here if needed, like setting the decal size
        }
        else
        {
            Debug.LogError("Decal prefab not assigned in DecalManager.");
        }
    }
}

using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform checkpoint;
    public GameObject swatterPrefab; // Reference to the swatter prefab
    private bool playerKilled = false; // Flag to track if player was killed

    public float killHeightUpper = 1.6f;
    public float killHeightLower = 0.45f;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float playerHeight = player.transform.position.y;
            if ((playerHeight > killHeightUpper || playerHeight < killHeightLower) && !playerKilled)
            {
                playerKilled = true;
                // Instantiate and activate the swatter
                GameObject swatterInstance = Instantiate(swatterPrefab, CalculateSwatterSpawnPosition(player.transform.position), swatterPrefab.transform.rotation);
                swatterInstance.GetComponent<Swatter>().ActivateSwatter(player.transform);

                Debug.Log("Player entered the kill zone. Swatter activated.");
            }
        }
    }

    private Vector3 CalculateSwatterSpawnPosition(Vector3 playerPosition)
    {
        // Adjust y position to be a bit above the player
        return new Vector3(playerPosition.x, playerPosition.y + 1f, playerPosition.z);
    }
}

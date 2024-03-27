using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform checkpoint;
    public GameObject swatterPrefab;
    public Transform playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
    }

    private Vector3 CalculateSwatterSpawnPosition(Vector3 playerPosition)
    {
        // Adjust y position to be a bit above the player
        return new Vector3(playerPosition.x, playerPosition.y + 1f, playerPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        if (other.gameObject.name == "Spider")
        {
            // Instantiate and activate the swatter
            GameObject swatterInstance = Instantiate(swatterPrefab, CalculateSwatterSpawnPosition(playerTransform.position), Quaternion.identity);
            swatterInstance.GetComponent<Swatter>().ActivateSwatter(playerTransform);

            Debug.Log("Player entered the kill zone. Swatter activated.");
        }

    }
}

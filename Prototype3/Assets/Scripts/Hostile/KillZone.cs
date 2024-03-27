using UnityEngine;

public class KillZone : MonoBehaviour
{
    public GameObject swatterPrefab;
    public Transform playerTransform;
    public UIManager uiManager;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == playerTransform)
        {
            uiManager.HideWarningMessage();
            // Instantiate and activate the swatter
            GameObject swatterInstance = Instantiate(swatterPrefab, CalculateSwatterSpawnPosition(playerTransform.position), Quaternion.identity);
            swatterInstance.GetComponent<Swatter>().ActivateSwatter(playerTransform);

            Debug.Log("Player entered the kill zone. Swatter activated.");
        }
    }

    private Vector3 CalculateSwatterSpawnPosition(Vector3 playerPosition)
    {
        // Adjust y position to be a bit above the player
        return new Vector3(playerPosition.x, playerPosition.y + 1f, playerPosition.z);
    }
}

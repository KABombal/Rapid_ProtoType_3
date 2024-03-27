using UnityEngine;

public class WarningZone : MonoBehaviour
{
    public UIManager uiManager;
    public Transform playerTransform;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in the scene!");
        }

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
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        if (other.gameObject.name == "Spider")
        {
            // Show warning message
            uiManager.ShowWarningMessage();
        }
        if (other.transform == playerTransform)
        {
            // Show warning message
            uiManager.ShowWarningMessage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Spider")
        {
            // Show warning message
            uiManager.HideWarningMessage();
        }
        if (other.transform == playerTransform)
        {
            // Hide warning message
            uiManager.HideWarningMessage();
        }
    }
}

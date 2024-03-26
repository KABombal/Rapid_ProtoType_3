using UnityEngine;

public class WarningZone : MonoBehaviour
{
    private UIManager uiManager;
    private bool warningShown = false;

    public float warningHeightUpper = 1.3f;
    public float warningHeightLower = 0.7f;
    public float killHeightUpper = 1.6f;
    public float killHeightLower = 0.45f;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>(); // Find the UIManager in the scene
        if (uiManager == null)
            Debug.LogError("UIManager not found in the scene!");
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float playerHeight = player.transform.position.y;
            if ((playerHeight > warningHeightUpper && playerHeight < killHeightUpper) ||
                (playerHeight < warningHeightLower && playerHeight > killHeightLower))
            {
                if (!warningShown)
                {
                    uiManager.ShowWarningMessage(); // Show warning message
                    warningShown = true;
                }
            }
            else
            {
                if (warningShown)
                {
                    uiManager.HideWarningMessage(); // Hide warning message
                    warningShown = false;
                }
            }
        }
    }
}

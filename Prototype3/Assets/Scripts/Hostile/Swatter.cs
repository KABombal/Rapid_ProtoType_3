using UnityEngine;

public class Swatter : MonoBehaviour
{
    public float speed = 0.5f;             // Speed of swatter's approach
    public AudioClip squashSound;          // Squash sound clip
    public Transform swatterTop;           // Transform of the top part of the swatter
    public SpiderController spiderController;

    public Vector3 spawnOffset = new Vector3(0, 10, 0); // Offset for the initial spawn position

    private bool isDescending = false;     // Flag to control swatter's movement
    public Transform target;              // Player's transform
    public AudioSource audioSource;       // Audio source for squash sound
    private float killDistance = 0.1f;     // Distance at which the swatter will kill the player

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDescending && target != null)
        {
            // Move the swatter towards the target's position
            Vector3 targetPosition = new Vector3(target.position.x, swatterTop.position.y, target.position.z);
            swatterTop.position = Vector3.MoveTowards(swatterTop.position, targetPosition, speed * Time.deltaTime);

            // Check the distance to the player
            if (Vector3.Distance(swatterTop.position, targetPosition) <= killDistance)
            {
                KillPlayer();
            }
        }
    }

    public void ActivateSwatter(Transform playerTransform)
    {
        target = playerTransform;
        // Set the initial position of the swatter at a specified offset
        transform.position = target.position + spawnOffset;
        swatterTop.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        isDescending = true;
    }

    private void KillPlayer()
    {
        // Play the squash sound
        if (audioSource != null && squashSound != null)
        {
            audioSource.PlayOneShot(squashSound);
        }

        // Call the player's death handling method
        if (spiderController != null)
        {
            spiderController.LoseLife();
        }
        else
        {
            Debug.LogError("SpiderController reference not set on Swatter.");
        }

        // Stop descending and disable or destroy the swatter
        isDescending = false;
        Invoke("Destroy(gameObject)", 1.5f);  // or use Destroy(gameObject); to completely remove the object
    }
}

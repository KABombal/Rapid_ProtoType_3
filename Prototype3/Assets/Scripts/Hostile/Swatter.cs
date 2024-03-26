using UnityEngine;

public class Swatter : MonoBehaviour
{
    public float speed = 1f;         // Speed of swatter's descent
    public AudioClip squashSound;    // Squash sound clip
    public Transform swatterTop;     // Transform of the top part of the swatter

    private bool isDescending = false;  // Flag to control swatter's movement
    private Transform target;           // Player's transform
    private AudioSource audioSource;    // Audio source for squash sound
    private float killDistance = 0.5f;  // Distance at which the swatter will kill the player
    public SpiderController spiderController; // Direct reference to the player controller script

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDescending && target != null)
        {
            // Move only the top of the swatter towards the player's position on the same height
            Vector3 targetPosition = new Vector3(target.position.x, swatterTop.position.y, target.position.z);
            swatterTop.position = Vector3.MoveTowards(swatterTop.position, targetPosition, speed * Time.deltaTime);

            // Check the distance to the player
            if (Vector3.Distance(swatterTop.position, targetPosition) < killDistance)
            {
                Debug.Log("Swatter Activated");
                KillPlayer();
            }
        }
    }

    public void ActivateSwatter(Transform playerTransform)
    {
        target = playerTransform;
        isDescending = true;
    }

    private void KillPlayer()
    {
        if (audioSource != null && squashSound != null)
        {
            audioSource.PlayOneShot(squashSound);
        }

        // Call the player's death handling method
        spiderController.LoseLife();

        // Stop descending and disable or destroy the swatter
        isDescending = false;
        gameObject.SetActive(false); // or Destroy(gameObject);
    }
}

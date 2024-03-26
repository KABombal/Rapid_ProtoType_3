using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swatter : MonoBehaviour
{
    public float speed = 8f;         // Speed of swatter's descent
    public AudioClip squashSound;    // Squash sound clip
    private bool isDescending = false;   // Flag to control swatter's movement
    private Transform target;            // Player's transform
    private AudioSource audioSource;     // Audio source for squash sound

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDescending && target != null)
        {
            // Move the swatter towards the player
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), speed * Time.deltaTime);
        }
    }

    public void ActivateSwatter(Transform playerTransform)
    {
        target = playerTransform;
        isDescending = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(squashSound);
            // Kill the player or trigger player's death logic
            collision.gameObject.GetComponent<SpiderController>().HandleParticleCollision();
            // Disable or destroy the swatter
            Destroy(gameObject);
        }
    }
}

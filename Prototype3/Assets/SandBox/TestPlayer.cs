using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public Transform checkpoint; // Assign the checkpoint transform in the Inspector
    private int lives = 3; // Example lives count
 
    public float speed = 5.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Make sure your spider has a Rigidbody component
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    public void HandleParticleCollision()
    {
        Debug.Log("Handling particle collision for player.");
        LoseLife();
        if (lives > 0)
        {
            // Respawn player at the checkpoint
            transform.position = checkpoint.position;
            Debug.Log("Player respawned at checkpoint. Lives remaining: " + lives);
        }
        else
        {
            // Handle game over logic
            Debug.Log("Game Over");
        }
    }

    void LoseLife()
    {
        lives--;
        Debug.Log("Life lost. Remaining lives: " + lives);
    }

}

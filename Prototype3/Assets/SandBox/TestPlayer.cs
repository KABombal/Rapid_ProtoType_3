using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public Transform checkpoint;
    private int lives = 3;
    private bool canTakeDamage = true;
    private float damageCooldown = 5.0f; // Cooldown time in seconds

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
        if (canTakeDamage)
        {
            LoseLife();
            Debug.Log("Player hit by spray. Lives remaining: " + lives);

            // Respawn player at the checkpoint
            transform.position = checkpoint.position;
            Debug.Log("Player respawned at checkpoint.");

            canTakeDamage = false;
            Invoke(nameof(ResetDamageCooldown), damageCooldown);
        }
    }


    void ResetDamageCooldown()
    {
        canTakeDamage = true;
    }

    void LoseLife()
    {
        lives--;
        Debug.Log("Life lost. Remaining lives: " + lives);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceshipController : MonoBehaviour
{
    public float maxSpeed = 5f;          // Maximum speed of the spaceship
    public float acceleration = 2f;      // How quickly the spaceship accelerates
    public float rotationOffset = -90f;  // Adjusts the ship's facing direction

    private Rigidbody2D rb;              // Rigidbody2D component for physics interactions
    private Vector2 currentVelocity;     // To store the current velocity for lerping

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from arrow keys or WASD
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Create a direction vector based on input
        Vector2 targetDirection = new Vector2(moveX, moveY).normalized;

        // Calculate target velocity based on direction and max speed
        Vector2 targetVelocity = targetDirection * maxSpeed;

        // Smoothly interpolate current velocity towards target velocity
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        // Apply the velocity to the Rigidbody2D
        rb.velocity = currentVelocity;

        // If there's movement, rotate the spaceship to face the direction of movement
        if (currentVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
            rb.rotation = angle + rotationOffset;
        }
    }
}

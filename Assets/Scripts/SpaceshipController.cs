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

    public int PlayerInputCount = 0;
    private Vector2 previousPosition;    // To store the previous position of the player
    public float TotalDistanceTraveled = 0f; // To store the total distance traveled

    private bool previousLeft;
    private bool previousRight;
    private bool previousUp;
    private bool previousDown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousLeft = false;
        previousRight = false;
        previousUp = false;
        previousDown = false;
    }

    void Update()
    {
        DetectUniqueKeystrokes();

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
        CalculateDistanceTravelled();
    }

    private void CalculateDistanceTravelled()
    {
        // Update the total distance traveled
        Vector2 currentPosition = rb.position;
        TotalDistanceTraveled += Vector2.Distance(previousPosition, currentPosition);
        previousPosition = currentPosition; // Update previous position
    }

    private void DetectUniqueKeystrokes()
    {
        // Get input from arrow keys or WASD
        bool currentLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool currentRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        bool currentUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        bool currentDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        // Check for unique keystrokes
        if (currentLeft != previousLeft || currentRight != previousRight || currentUp != previousUp || currentDown != previousDown)
        {
            PlayerInputCount++;
            previousLeft = currentLeft;
            previousRight = currentRight;
            previousUp = currentUp;
            previousDown = currentDown;
        }
    }

}

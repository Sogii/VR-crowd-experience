using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionController : MonoBehaviour
{
    public ScoreManager scoreManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("yese");
        // Check if the object collided with is tagged as "Coin"
        if (other.gameObject.CompareTag("Coin"))
        {
            // Increment score by the coin's value
            scoreManager.CollectCoin();

            // Destroy the coin
            Destroy(other.gameObject);
        }
    }
}

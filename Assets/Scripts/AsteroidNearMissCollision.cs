using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteriodNearMissCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AsteroidBehavior asteroid = GetComponentInParent<AsteroidBehavior>();
            asteroid.InniatiateNearMiss();
        }
    }
}

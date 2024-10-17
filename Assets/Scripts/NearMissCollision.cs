using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearMissCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CoinBehavior coin = GetComponentInParent<CoinBehavior>();
            coin.InniatiateNearMiss();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.PlayerHit();
            EventManager.ScoreChanged(50);         //Modifier can be separated into a variable
            Destroy(this.gameObject);
        }
    }
}

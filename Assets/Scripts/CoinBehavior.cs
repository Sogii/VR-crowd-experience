using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    [SerializeField] private int intScoreIncrease = 100;
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.ScoreChanged(intScoreIncrease);         //Modifier can be separated into a variable
            SoundManager.instance.PlaySound(collectSound);
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    private int intScoreIncrease = 200;
    [SerializeField] private AudioClip collectSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.MultiplierChanged(0.2f);
            EventManager.ScoreChanged(intScoreIncrease);         //Modifier can be separated into a variable
            SoundManager.instance.PlaySound(collectSound);
            Destroy(this.gameObject);
        }
    }

    private void Awake()
    {
        EventManager.CoinCountChanged(1);
    }

    private void OnDestroy()
    {
        EventManager.CoinCountChanged(-1);
    }
}

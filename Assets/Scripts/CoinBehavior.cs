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

            isNearMissActive = false;                            // Stop the near miss timer if the coin is collected

            EventManager.MultiplierChanged(0.2f);
            EventManager.ScoreChanged(intScoreIncrease);         
            SoundManager.instance.PlaySound(collectSound);

            //For data collection
            DataFetcher.Instance.TimeStepCoinsCollected++;

            Destroy(this.gameObject);
        }
    }

    public float CalculateDistanceToPlayer(GameObject player)
    {
        return Vector2.Distance(player.transform.position, this.transform.position);
    }

    private bool isNearMissActive = false;
    private float nearMissTimer = 0.0f;
    private float nearMissCountdown = 1.5f;
    public void InniatiateNearMiss()
    {
        isNearMissActive = true;
        nearMissTimer = nearMissCountdown;
    }

    private void Update()
    {
        if (isNearMissActive)
        {
            nearMissTimer -= Time.deltaTime;
            if (nearMissTimer <= 0)
            {
                DataFetcher.Instance.TimeStepCoinNearMiss++;
                isNearMissActive = false;
            }
        }
    }

    private void Awake()
    {
        DataFetcher.Instance.PopulateCoinsOnMap(this.gameObject);
        EventManager.CoinCountChanged(1);
    }

    private void OnDestroy()
    {
        DataFetcher.Instance.RemoveCoinFromMap(this.gameObject);
        EventManager.CoinCountChanged(-1);
    }
}

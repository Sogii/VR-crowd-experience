using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    [SerializeField] private GameObject _gameBounds;
    private float spawnDelaySeconds = 2f;

    void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelaySeconds);
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        GameObject coin = Instantiate(Resources.Load("Prefabs/Coin"), GetRandomPositionWithinBounds(), Quaternion.identity) as GameObject;
    }

    private Vector2 GetRandomPositionWithinBounds()
    {
        Vector2[] corners = PlaneBoundsUtilities.GetPlaneCorners(_gameBounds);
        //Genreate random point within corners (minus buffer amount)
        float bufferamount = 0.5f;
        float x = Random.Range(corners[0].x + bufferamount, corners[2].x - bufferamount);
        float y = Random.Range(corners[0].y + bufferamount, corners[2].y - bufferamount);
        return new Vector2(x, y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    [SerializeField] private BoxCollider2D _gameBounds;
    private float spawnDelaySeconds = 2f;
    [SerializeField] private int coinCount = 0;
    [SerializeField] private int maxCoins = 5;

    void Awake()
    {
        EventManager.OnCoinCountChanged += CoinCountChanged;
    }

    private void CoinCountChanged(int changeAmount)
    {
        coinCount += changeAmount;
    }

    void Start()
    {
        StartCoroutine(SpawnCoins());
    }



    IEnumerator SpawnCoins()
    {
        while (true && coinCount < maxCoins)
        {
            yield return new WaitForSeconds(spawnDelaySeconds);
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        GameObject coin = Instantiate(Resources.Load("Prefabs/Coin"), GetRandomPositionWithinBounds(), Quaternion.identity) as GameObject;
    }

    private void DespawnCoins()
    {

    }

    private Vector2 GetRandomPositionWithinBounds()
    {
        Vector2[] corners = GetRectangleCorners();
        //Genreate random point within corners (minus buffer amount)
        float bufferamount = 0.5f;
        float x = Random.Range(corners[0].x + bufferamount, corners[2].x - bufferamount);
        float y = Random.Range(corners[0].y + bufferamount, corners[2].y - bufferamount);
        return new Vector2(x, y);
    }

    private Vector2[] GetRectangleCorners()
    {
        Vector2[] corners = new Vector2[4];

        // Calculate the half-width and half-height
        float halfWidth = _gameBounds.size.x / 2;
        float halfHeight = _gameBounds.size.y / 2;

        // Get the center position of the collider
        Vector2 center = (Vector2)transform.position + _gameBounds.offset;

        // Calculate the corners
        corners[0] = center + new Vector2(-halfWidth, halfHeight);  // Top Left
        corners[1] = center + new Vector2(halfWidth, halfHeight);   // Top Right
        corners[3] = center + new Vector2(-halfWidth, -halfHeight); // Bottom Left
        corners[2] = center + new Vector2(halfWidth, -halfHeight);  // Bottom Right

        return corners;
    }
}

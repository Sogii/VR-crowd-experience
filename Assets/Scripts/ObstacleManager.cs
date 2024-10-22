using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //[SerializeField] private float _spawnDistance = 1.35f;
    [SerializeField] private BoxCollider2D _gameBounds;
    [SerializeField] private float _asteroidSpawnTime = 5f;
    private float _difficultyModifier = 1f;
    private bool booRunning = false;


    void Awake()
    {
        EventManager.OnDifficultyChanged += DifficultyChanged;
    }

    private void DifficultyChanged(float multiPlier)
    {
        //Ranges from 1.5 to 25 roughyly based on multiplier
        float a = 0.5f;
        float b = 0.5f;
        _difficultyModifier = a * Mathf.Exp(b * multiPlier);
        _asteroidSpawnTime = 8f / (_difficultyModifier * 3);
    }

    void Start()
    {
        StartCoroutine(AsteroidSpawner());
        booRunning = true;
    }

    public void Restart()
    {
        booRunning = true;
    }

    public void Stop()
    {
        GameObject[] enemiesToClear = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemiesToClear)
        {
            Destroy(obj);
        }
        booRunning = false;
    }

    IEnumerator AsteroidSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(_asteroidSpawnTime);
            if (booRunning)
            {
                SpawnRandomAsteroid();
            }
        }
    }

    private void SpawnAsteroid(float speed, Vector2[] path, float size)
    {
        GameObject asteroid = Instantiate(Resources.Load("Prefabs/NormalAsteroid"), path[0], Quaternion.identity) as GameObject;
        asteroid.GetComponent<AsteroidBehavior>().Innitialize(speed, path, size);
    }

    private void SpawnRandomAsteroid()
    {
        float maxSpeed = 4f;
        float minSpeed = .5f;
        float mean = (maxSpeed + minSpeed) / 2;
        float stdDev = (maxSpeed - mean) / 2;


        float speed = ProbabilityUtlities.GenerateRightHalfNormalRandomValue(minSpeed, maxSpeed, mean, stdDev);
        //Size, a higher speed means a smaller size ranging from 1 to 15
        float size = Mathf.Lerp(.2f, 10, (speed - minSpeed) / (maxSpeed - minSpeed));


        Vector2[] asteroidPath = GetRandomPathVectorOnBounds(GetRectangleCorners());
        //adjust for difficulty
        speed *= 1 + (Mathf.Log((_difficultyModifier / 2) + 1) / Mathf.Log(50));
        // size *= _difficultyModifier;

        SpawnAsteroid(speed, asteroidPath, size);
    }

    Vector2[] GetRandomPathVectorOnBounds(Vector2[] corners)
    {
        //Pick a random side and direction
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: //Up
                return new Vector2[] { RandomPointOnLine(corners[0], corners[1]), RandomPointOnLine(corners[2], corners[3]) };
            case 1: // Left
                return new Vector2[] { RandomPointOnLine(corners[1], corners[2]), RandomPointOnLine(corners[3], corners[0]) };
            case 2: // Down
                return new Vector2[] { RandomPointOnLine(corners[2], corners[3]), RandomPointOnLine(corners[0], corners[1]) };
            case 3: // Right
                return new Vector2[] { RandomPointOnLine(corners[3], corners[0]), RandomPointOnLine(corners[2], corners[1]) };
            default:
                return new Vector2[] { Vector2.zero, Vector2.zero };
        }
    }

    private Vector2 RandomPointOnLine(Vector2 pointA, Vector2 pointB)
    {
        return Vector2.Lerp(pointA, pointB, Random.Range(0.0f, 1.0f));
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

    private void OnDestroy()
    {
        EventManager.OnDifficultyChanged -= DifficultyChanged;
    }
}
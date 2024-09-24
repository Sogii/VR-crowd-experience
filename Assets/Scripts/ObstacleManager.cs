using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private float _spawnDistance = 1.35f;
    [SerializeField] private GameObject _gameBounds;
    [SerializeField] private float _asteroidSpawnTime = 2f;

    void Start()
    {
        StartCoroutine(AsteroidSpawner());
    }

    IEnumerator AsteroidSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(_asteroidSpawnTime);
            SpawnRandomAsteroid();
        }
    }

    private void SpawnAsteroid(float speed, Vector2[] path, float size)
    {
        GameObject asteroid = Instantiate(Resources.Load("Prefabs/NormalAsteroid"), path[0], Quaternion.identity) as GameObject;
        asteroid.GetComponent<AsteroidBehavior>().Innitialize(speed, path, size);
    }

    private void SpawnRandomAsteroid()
    {
        Vector2[] asteroidPath = GetRandomPathVectorOnBounds(GetOuterPlaneCorners(GetPlaneCorners(_gameBounds)));
        float speed = Random.Range(1, 7);
        float size = 20 - speed * 2;
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

    Vector2[] GetPlaneCorners(GameObject plane)
    {
        Vector2[] corners = new Vector2[4];
        Transform transform = plane.transform;
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 scale = new Vector2(transform.localScale.x, transform.localScale.y);
        Quaternion rotation = transform.rotation;

        // Calculate the half extents
        Vector2 halfExtents = new Vector2(scale.x / 2, scale.y / 2);

        // Calculate the corners
        corners[0] = position + (Vector2)(rotation * new Vector2(-halfExtents.x, -halfExtents.y)); // Bottom-left
        corners[1] = position + (Vector2)(rotation * new Vector2(halfExtents.x, -halfExtents.y));  // Bottom-right
        corners[2] = position + (Vector2)(rotation * new Vector2(halfExtents.x, halfExtents.y));   // Top-right
        corners[3] = position + (Vector2)(rotation * new Vector2(-halfExtents.x, halfExtents.y));  // Top-left

        return corners;
    }

    Vector2[] GetOuterPlaneCorners(Vector2[] innerPlaneCorners)
    {
        Vector2[] outerPlaneCorners = new Vector2[4];
        for (int i = 0; i < innerPlaneCorners.Length; i++)
        {
            outerPlaneCorners[i] = innerPlaneCorners[i] * _spawnDistance;
        }
        return outerPlaneCorners;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //[SerializeField] private float _spawnDistance = 1.35f;
    [SerializeField] private BoxCollider2D _gameBounds;
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
        //Vector2[] asteroidPath = GetRandomPathVectorOnBounds(GetOuterPlaneCorners(PlaneBoundsUtilities.GetPlaneCorners(_gameBounds)));
        Vector2[] asteroidPath = GetRandomPathVectorOnBounds(GetRectangleCorners());
        float speed = ProbabilityUtlities.GenerateRightHalfNormalRandomValue(1f, 10f, 3f, 2f);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] float spawnDistance = 10;
    void Start()
    {
        StartCoroutine(AsteroidSpawner());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AsteroidSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            SpawnAsteroid(4, GenerateRandomNornamlizedDirectionVector());
        }
    }

    private void SpawnAsteroid(int speed, Vector2 direction)
    {
        GameObject asteroid = Instantiate(Resources.Load("Prefabs/NormalAsteroid"), CalculateAsteroidSpawnPosition(direction), Quaternion.identity) as GameObject;
        asteroid.GetComponent<AsteroidBehavior>().Initialzize(speed, direction, Random.Range(5, 15));
    }

    private Vector2 CalculateAsteroidSpawnPosition(Vector2 asteroidNormalVector)
    {
        //Spawn the asteroid just outside of the screen, opposite of the normal vector direction angle
        return new Vector2(asteroidNormalVector.x * -spawnDistance, asteroidNormalVector.y * -spawnDistance);

    }

    private Vector2 GenerateRandomNornamlizedDirectionVector()
    {
        float angle = Random.Range(0, 360);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}

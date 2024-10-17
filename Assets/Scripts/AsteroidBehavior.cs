using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    [SerializeField] private GameObject asteroidParticles;
    [SerializeField] private AudioClip hitSound;
    private float _speed;
    private Vector2[] path;
    private Vector2 _directionvector;
    private float size;

    public void Innitialize(float speed, Vector2[] path, float size)
    {
        _speed = speed;
        this.path = path;
        _directionvector = CalculateNormalizedDirectionVector(path);
        this.size = size;
        transform.localScale = new Vector3(size, size, 1);
        if (asteroidParticles == null)
        {
            Debug.LogWarning("No particlesystem object found");
        }
    }

    public float CalculateDistanceToPlayer(GameObject player)
    {
        return Vector2.Distance(player.transform.position, this.transform.position);
    }

    static Vector2 CalculateNormalizedDirectionVector(Vector2[] path)
    {
        Vector2 direction = path[1] - path[0];
        return direction.normalized;
    }

    void Update()
    {
        Movement();
        CheckForSelfDestruct();
        NearMissCountDown();
    }

    private bool isNearMissActive = false;
    private float nearMissTimer = 0.0f;
    private float nearMissCountdown = 1.5f;
    public void InniatiateNearMiss()
    {
        isNearMissActive = true;
        nearMissTimer = nearMissCountdown;
    }

    private void NearMissCountDown()
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isNearMissActive = false;                            // Stop the near miss timer if the asteroid is hit
            EventManager.PlayerHit();
            EventManager.ScoreChanged(-10);                      //Modifier can be separated into a variable
            GameObject objParticles = Instantiate(asteroidParticles, this.gameObject.transform.position, asteroidParticles.transform.rotation);
            DataFetcher.Instance.TimeStepHitByAsteroids++;
            SoundManager.instance.PlaySound(hitSound);
            Destroy(this.gameObject);
        }
    }

    private void Movement()
    {
        transform.Translate((Vector2)_directionvector * _speed * Time.deltaTime);
    }

    private void CheckForSelfDestruct()
    {
        //If object surpasses the end of the path destroy it
        if (Vector2.Distance(transform.position, path[1]) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        DataFetcher.Instance.PopulateAsteroidOnMap(this.gameObject);
    }

    void OnDestroy()
    {
        DataFetcher.Instance.RemoveAsteroidFromMap(this.gameObject);
    }
}
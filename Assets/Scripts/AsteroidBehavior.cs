using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.PlayerHit();
            EventManager.ScoreChanged(-10);         //Modifier can be separated into a variable
            Destroy(this.gameObject);

            //Trigger collision results in "ScoreManager" script
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
}

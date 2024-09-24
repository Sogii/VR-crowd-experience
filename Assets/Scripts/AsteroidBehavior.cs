using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    private float _speed;
    private Vector2Int _directionvector;

    public void Initialzize(float speed, Vector2Int direction)
    {
        _speed = speed;
        _directionvector = direction;
    }

    void Update()
    {
        Movement();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);

            //Trigger collision results in "ScoreManager" script
        }
    }

    private void Movement()
    {
        transform.Translate((Vector2)_directionvector * _speed * Time.deltaTime);
    }
}

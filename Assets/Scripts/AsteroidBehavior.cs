using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    private float _speed;
    private Vector2 _directionvector;
    private float size;

    public void Initialzize(float speed, Vector2 direction, float size)
    {
        _speed = speed;
        _directionvector = direction;
        this.size = size;
        transform.localScale = new Vector3(size, size, 1);
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

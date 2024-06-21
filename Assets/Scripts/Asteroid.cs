using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] public float size = 1.0f;
    [SerializeField] public float minSize = 0.5f;
    [SerializeField] public float maxSize = 1.5f;
    [SerializeField] private float speed = 50.0f;
    [SerializeField] private float maxLifetime = 30.0f;
    [SerializeField] private float health = 30.0f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        transform.localScale = Vector3.one * size;
        _rigidbody.mass = size;

        Destroy(gameObject, maxLifetime);
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * speed);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) {
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        if ((size * 0.5f) >= minSize) {
            CreateSplit();
            CreateSplit();
        }

        FindObjectOfType<GameManager>().AsteroidDestroyed(this);
        Destroy(gameObject);
    }

    private void CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized);
    }
}

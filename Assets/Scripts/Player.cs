using System.Collections;
using System.Collections.Generic;
using  UnityEngine;
     
public class Player : MonoBehaviour
{
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float boostAcceleration = 1000f;
    [SerializeField] private float shipMaxVelocity = 20f;
    [SerializeField] private float shipRotationSpeed = 360f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float pulseExpansionRate = 8f;
    

    [Header("Object references")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private GameObject ringPrefab;

    private Rigidbody2D shipRigidbody;
    public bool isAlive = true;
    private bool isAccelerating = false;
    private bool isDecelerating = false;
    private bool isBoosting = false;
    private bool isShooting = false;


    private void Start()
    {
        // Get a reference to the attached RigidBody2D
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
 
        if (isAlive) {
            HandleShipAcceleration();
            HandleShipBoost();
            HandleShipRotation();
            HandleShooting();
            HandlePulse();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && isAccelerating) {
            // Increase velocity to a maximum
            shipRigidbody.AddForce(shipAcceleration * transform.up);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
        else if  (isAlive && isDecelerating) {
            shipRigidbody.AddForce(-shipAcceleration * transform.up);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
    }

    private void HandleShipAcceleration()
    {
        isAccelerating = Input.GetKey("w");
        isDecelerating = Input.GetKey("s");
        isBoosting = Input.GetKey("f");
    }

    private void HandleShipBoost()
    {
        if (isAlive && isBoosting) {
            shipRigidbody.AddForce(boostAcceleration * transform.up, ForceMode2D.Impulse);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
    }

    private void HandleShipRotation()
    {
        if (Input.GetKey("a")) {
            transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey("d")) {
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKey(KeyCode.Space) && !isShooting) {
            isShooting = true;
            
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            
            // Directly add ship's velocity to bullet's velocity
            bullet.velocity = shipRigidbody.velocity;
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
        else if (!Input.GetKey(KeyCode.Space)) {
            isShooting = false;
        }
    }

    private void HandlePulse()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            GameObject ring = Instantiate(ringPrefab, transform.position, Quaternion.identity);
            StartCoroutine(ExpandRing(ring));
        }
    }

    public IEnumerator ExpandRing(GameObject ring)
    {
        float startTime = Time.time;
        float duration = 5f;

        while (Time.time < startTime + duration) {
            float elapsed = Time.time - startTime;
            ring.transform.localScale = Vector3.one * (1 + elapsed * pulseExpansionRate);

            yield return null;
        }

        Destroy(ring);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid")) {
            shipRigidbody.velocity = Vector3.zero;
            shipRigidbody.angularVelocity = 0.0f;
            isAlive = false;
            transform.position = new Vector3(-9999, -9999, transform.position.z);

            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}

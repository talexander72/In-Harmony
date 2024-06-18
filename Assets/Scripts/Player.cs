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
    [SerializeField] private float perfectTimingWindow = 0.1f;


    [Header("Object references")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private RhythmManager rhythmManager;


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
            isShooting = true; // one input = one attack
            
            // initialize rhythm tracking and damage multiplier variables:
            float attackTime = rhythmManager.GetTime();
            float closestBeat = rhythmManager.GetClosestBeatTime(attackTime);
            float timingDifference = Mathf.Abs(attackTime - closestBeat);
            float damageMultiplier = CalculateDamageMultiplier(timingDifference);
            Debug.Log("Shooting triggered with damage multiplier: " + damageMultiplier);

            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            
            // directly add ships velocity to bullet initial velocity:
            bullet.velocity = shipRigidbody.velocity;
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
            bullet.GetComponent<Bullet>().SetDamageMultiplier(damageMultiplier);
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


    private float CalculateDamageMultiplier(float timingDifference)
    {
        if (timingDifference < perfectTimingWindow) {
            return 2.0f; // perfect hit
        } else if (timingDifference < perfectTimingWindow * 2) {
            return 1.5f; // good hit
        } else {
            return 1.0f; // regular hit
        }
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

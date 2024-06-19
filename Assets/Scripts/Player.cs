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
    [SerializeField] public static float perfectTimingWindow = 0.1f;

    [Header("Object references")]
    [SerializeField] private BulletManager bulletManager;
    [SerializeField] private PulseManager pulseManager;
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
            HandleInput();
            HandleShooting();
            HandlePulse();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive) {
            HandleMovement();
        }
    }
    
    private void HandleInput()
    {
        isAccelerating = Input.GetKey("w");
        isDecelerating = Input.GetKey("s");
        isBoosting = Input.GetKey("f");

        if (Input.GetKey("a")) {
            transform.Rotate(shipRotationSpeed * Time.deltaTime * Vector3.forward);
        }
        else if (Input.GetKey("d")) {
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    private void HandleMovement()
    {
        if (isAccelerating) {
            shipRigidbody.AddForce(shipAcceleration * transform.up);
        } else if (isDecelerating) {
            shipRigidbody.AddForce(-shipAcceleration * transform.up);
        }

        if (isBoosting) {
            shipRigidbody.AddForce(boostAcceleration * transform.up, ForceMode2D.Impulse);
        }

        shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
    }

    private void HandleShooting()
    {
        if (Input.GetKey(KeyCode.Space) && !isShooting) {
            isShooting = true; // one input = one attack
            bulletManager.TriggerBullet(transform.position);
        }
        else if (!Input.GetKey(KeyCode.Space)) {
            isShooting = false;
        }
    }

//    private void HandleMinigun()
//   {
//        if (Input.GetKey(KeyCode.Space)) {
//           bulletManager.TriggerBullet(transform.position);
//        }
//    }

    private void HandlePulse()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            pulseManager.TriggerPulse(transform.position);
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

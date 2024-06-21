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
    private float accelerationInput = 0;
    private Vector2 rotationInput = Vector2.zero;


    private void Start()
    {
        // Get a reference to the attached RigidBody2D
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAlive) {
            //HandleKeyboardInput();
            HandleShooting();
            HandlePulse();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive) {
            //HandleKeyboardMovement();
            HandleTouchMovement();
        }
    }


    // keyboard inputs
    //private void HandleKeyboardInput()
    //{
    //    isAccelerating = Input.GetKey("w");
    //    isDecelerating = Input.GetKey("s");
    //    isBoosting = Input.GetKey("f");

    //    if (Input.GetKey("a")) {
    //        transform.Rotate(shipRotationSpeed * Time.deltaTime * Vector3.forward);
    //    }
    //    else if (Input.GetKey("d")) {
    //        transform.Rotate(-shipRotationSpeed * Time.deltaTime * Vector3.forward);
    //    }
    //}

    //private void HandleKeyboardMovement()
    //{
    //    if (isAccelerating) {
    //        shipRigidbody.AddForce(shipAcceleration * transform.up);
    //    } else if (isDecelerating) {
    //        shipRigidbody.AddForce(-shipAcceleration * transform.up);
    //    }

    //    if (isBoosting) {
    //        shipRigidbody.AddForce(boostAcceleration * transform.up, ForceMode2D.Impulse);
    //    }

    //    shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
    //}
    

    // joystick inputs
    public void SetAcceleration(float value)
    {
        accelerationInput = value;
    }

    public void SetRotation(Vector2 value)
    {
        rotationInput = value;
    }

    private void HandleTouchMovement()
    {
        // Apply forward force based on acceleration input
        shipRigidbody.AddForce(accelerationInput * shipAcceleration * transform.up);
        shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);

        // Rotate based on the rotation input
        float angle = Mathf.Atan2(rotationInput.y, rotationInput.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void HandleShooting()
    {
        // keyboard control:
        if (Input.GetKey(KeyCode.Space) && !isShooting) {
            isShooting = true; // one input = one attack
            bulletManager.TriggerBullet(transform.position);
        }
        else if (!Input.GetKey(KeyCode.Space)) {
            isShooting = false;
        }

        // touchscreen control:

    }

    private void HandlePulse()
    {
        // keyboard control:
        if (Input.GetKeyDown(KeyCode.R)) {
            pulseManager.TriggerPulse(transform.position);
        }  

        // touchscreen control:

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

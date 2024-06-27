using System.Collections;
using System.Collections.Generic;
using  UnityEngine;
     

public class Player : MonoBehaviour
{
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    //[SerializeField] private float boostAcceleration = 1000f;
    [SerializeField] private float shipMaxVelocity = 20f;
    //[SerializeField] private float shipRotationSpeed = 360f;

    [Header("Object references")]
    [SerializeField] private BulletManager bulletManager;
    [SerializeField] private PulseManager pulseManager;
    [SerializeField] private RhythmManager rhythmManager;

    private Rigidbody2D shipRigidbody;
    public bool isAlive = true;
    //private bool isBoosting = false;
    //private bool isShooting = false;
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
            HandleShooting();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive) {
            HandleMovement();
        }
    }


    // joystick inputs
    public void SetAcceleration(float value)
    {
        accelerationInput = value;
    }

    public void SetRotation(Vector2 value)
    {
        rotationInput = value;
    }

    private void HandleMovement()
    {
        // Apply forward force based on acceleration input
        shipRigidbody.AddForce(accelerationInput * shipAcceleration * transform.up);
        shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);

        // Rotate based on the rotation input
        if (rotationInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(rotationInput.y, rotationInput.x) * Mathf.Rad2Deg - 90f;
            shipRigidbody.rotation = angle;
        }
    }

    private void HandleShooting()
    {
        // keyboard control:
        //if (Input.GetKey(KeyCode.Space) && !isShooting) {
        //    isShooting = true; // one input = one attack
        //    bulletManager.TriggerBullet(transform.position);
        //}
        //else if (!Input.GetKey(KeyCode.Space)) {
        //    isShooting = false;
        //}

        // touchscreen control:
        //if (Input.touchCount > 1)
        //{
        //    Touch touch = Input.GetTouch(1);
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        bulletManager.TriggerBullet(transform.position);
        //    }
        //}
    }

    public void TriggerPulse()
    {
        pulseManager.TriggerPulse(transform.position);
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

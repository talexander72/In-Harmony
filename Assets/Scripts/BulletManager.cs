using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private RhythmManager rhythmManager;

    [SerializeField] private float bulletLifetime = 1.0f;
    [SerializeField] public float initialDamage = 15.0f;
    [SerializeField] private float bulletSpeed = 50.0f;


    public void TriggerBullet(Vector3 position) 
    {
        float multiplier = rhythmManager.MeasureInputs();
        float adjustedDamage = rhythmManager.AdjustDamage(initialDamage, multiplier);
        
        Rigidbody2D bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        Destroy(bullet.gameObject, bulletLifetime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();
            
            if (asteroid != null)
            {
                asteroid.TakeDamage(initialDamage);
                Destroy(gameObject); // Destroy the bullet on impact
            }
        }
    }
}

 
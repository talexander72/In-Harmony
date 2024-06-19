using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private float bulletLifetime = 1.0f;
    [SerializeField] public float bulletDamage = 15.0f;
    [SerializeField] public float bulletSpeed = 50.0f;


    public void TriggerBullet(Vector3 position) 
    {
        bulletDamage = rhythmManager.AdjustDamage(bulletDamage);
        Debug.Log("This bullet dealt " + bulletDamage + " hp on damage");
        
        Rigidbody2D bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        Destroy(bullet.gameObject, bulletLifetime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}

 
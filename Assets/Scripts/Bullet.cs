using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 1.0f;
    [SerializeField] private float bulletDamage = 15.0f;

    // When created, destroy after set amount of time
    private void Awake()
    {
        Destroy(gameObject, bulletLifetime);
    }

    public void SetDamageMultiplier(float damageMultiplier)
    {
        bulletDamage *= damageMultiplier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    

}

 
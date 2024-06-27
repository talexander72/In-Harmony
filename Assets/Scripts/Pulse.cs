using UnityEngine;

public class Pulse: MonoBehaviour 
{
    public float adjustedDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.TakeDamage(adjustedDamage);
            }
        }
    }
}
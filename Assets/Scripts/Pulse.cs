using System.Collections;
using UnityEngine;


public class Pulse: MonoBehaviour 
{
    public float adjustedDamage;
    public float adjustedExpansionRate;
    public float adjustedLifetime;

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


    public IEnumerator ExpandRing(GameObject pulse)
    {
        float startTime = Time.time;

        while (Time.time < startTime + adjustedLifetime) {
            float elapsed = Time.time - startTime;
            pulse.transform.localScale = Vector3.one * (1 + elapsed * adjustedExpansionRate);
            yield return null;
        }

        Destroy(pulse);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseManager: MonoBehaviour
{
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private float pulseLifetime = 3.0f;
    [SerializeField] public float pulseDamage = 15.0f;
    [SerializeField] private float pulseExpansionRate = 8.0f;
    

    public void TriggerPulse(Vector3 position) {
            float adjustedDamage = rhythmManager.AdjustDamage(pulseDamage);
            Debug.Log("This pulse did " + adjustedDamage + " damage");
            
            GameObject pulse = Instantiate(pulsePrefab, position, Quaternion.identity);
            StartCoroutine(ExpandRing(pulse));
    }
    
    private IEnumerator ExpandRing(GameObject pulse)
    {
        float startTime = Time.time;

        while (Time.time < startTime + pulseLifetime) {
            float elapsed = Time.time - startTime;
            pulse.transform.localScale = Vector3.one * (1 + elapsed * pulseExpansionRate);
            yield return null;
        }

        Destroy(pulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.TakeDamage(pulseDamage);
            }
        }
    }

}

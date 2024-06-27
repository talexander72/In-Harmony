using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PulseManager: MonoBehaviour
{
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private float pulseLifetime = 3.0f;
    [SerializeField] public float initialDamage = 15.0f;
    [SerializeField] private float pulseExpansionRate = 8.0f;
    
    [SerializeField] private Color lowDamageColor = Color.green;
    [SerializeField] private Color mediumDamageColor = Color.yellow;
    [SerializeField] private Color highDamageColor = Color.red;

    public void TriggerPulse(Vector3 position) {
            float adjustedDamage = rhythmManager.AdjustDamage(initialDamage);
            Debug.Log("This pulse did " + adjustedDamage + " damage");
            
            GameObject pulse = Instantiate(pulsePrefab, position, Quaternion.identity);
            Pulse pulseComponent = pulse.GetComponent<Pulse>();
            if (pulseComponent != null) {
                pulseComponent.adjustedDamage = adjustedDamage;
                SetPulseColor(pulse, adjustedDamage);
            }
            StartCoroutine(ExpandRing(pulse));
    }
    
    private void SetPulseColor(GameObject pulse, float adjustedDamage)
    {
        SpriteRenderer renderer = pulse.GetComponent<SpriteRenderer>();
        if (renderer != null) {
            Color pulseColor;

            if (adjustedDamage >= initialDamage * rhythmManager.highMultiplier) {
                pulseColor = highDamageColor;
            }
            else if (adjustedDamage >= initialDamage * rhythmManager.mediumMultiplier) {
                pulseColor = mediumDamageColor;
            }
            else {
                pulseColor = lowDamageColor;
            }
            renderer.color = pulseColor;
            }
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
}

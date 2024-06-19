using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseManager: MonoBehaviour
{
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private float pulseLifetime = 3.0f;
    [SerializeField] private float pulseDamage = 15.0f;
    [SerializeField] private float pulseExpansionRate = 8.0f;
    

    public void TriggerPulse(Vector3 position) {
            pulseDamage *= rhythmManager.AdjustDamage(pulseDamage);
            GameObject pulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity);
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

     public void SetDamageMultiplier(float damageMultiplier)
    {
        pulseDamage *= damageMultiplier;
    }
}

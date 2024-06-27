using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class PulseManager: MonoBehaviour
{

    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private RhythmManager rhythmManager; 

    [SerializeField] public float initialDamage = 15.0f;
    [SerializeField] private float initialExpansionRate = 8.0f;
    [SerializeField] private float initialLifetime = 2.0f;

    [SerializeField] private Color lowDamageColor = Color.white;
    [SerializeField] private Color mediumDamageColor = Color.yellow;
    [SerializeField] private Color highDamageColor = Color.red;


    public void TriggerPulse(Vector3 position) 
    {
        float multiplier = rhythmManager.MeasureInputs();
        float adjustedDamage = rhythmManager.AdjustDamage(initialDamage, multiplier);
        float adjustedExpansionRate = rhythmManager.AdjustExpansionRate(initialExpansionRate, multiplier);
        float adjustedLifetime = rhythmManager.AdjustLifetime(initialLifetime, multiplier);
        
        GameObject pulse = Instantiate(pulsePrefab, position, Quaternion.identity);
        Pulse pulseComponent = pulse.GetComponent<Pulse>();

        if (pulseComponent != null) 
        {
            pulseComponent.adjustedDamage = adjustedDamage;
            pulseComponent.adjustedExpansionRate = adjustedExpansionRate;
            pulseComponent.adjustedLifetime = adjustedLifetime;
            SetPulseColor(pulse, adjustedDamage);
        }

        pulseComponent.StartCoroutine(pulseComponent.ExpandRing(pulse));
    }
    

    private void SetPulseColor(GameObject pulse, float adjustedDamage)
    {
        SpriteRenderer renderer = pulse.GetComponent<SpriteRenderer>();
        
        if (renderer != null) 
        {
            Color pulseColor;

            if (adjustedDamage >= initialDamage * rhythmManager.highMultiplier) 
                {pulseColor = highDamageColor;}

            else if (adjustedDamage >= initialDamage * rhythmManager.mediumMultiplier) 
                {pulseColor = mediumDamageColor;}

            else 
                {pulseColor = lowDamageColor;}
            
            renderer.color = pulseColor;
        }
    }
    

}

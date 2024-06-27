using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RhythmManager : MonoBehaviour 
{
    [SerializeField] public float lowMultiplier = 0.5f;
    [SerializeField] public float mediumMultiplier = 1.0f;
    [SerializeField] public float highMultiplier = 3.0f;
    [SerializeField] public float perfectTimingWindow = 0.1f;

    public AudioSource audioSource;
    public float[] beatTimes; // Predefined beat times in seconds


    private void Start() {
        audioSource.Play();
    }


    public float GetTime() {
        return audioSource.time; 
    }


    public float GetClosestBeatTime(float time) {
        float closestBeat = beatTimes[0];
        float minDifference = Mathf.Abs(time - closestBeat);

        foreach (float beat in beatTimes) {
            float difference = Mathf.Abs(time - beat);
            
            if (difference < minDifference) 
            {
                closestBeat = beat;
                minDifference = difference;
            }
        }

        return closestBeat;
    }


    public float CalculateDamageMultiplier(float timingDifference)
    {
        if (timingDifference < perfectTimingWindow) 
            {return highMultiplier;} 
        else if (timingDifference < perfectTimingWindow * 2)
            {return mediumMultiplier;}
        else 
            {return lowMultiplier;}
    }


    public float MeasureInputs()
    {
        float attackTime = GetTime();
        float closestBeat = GetClosestBeatTime(attackTime);
        float timingDifference = Mathf.Abs(attackTime - closestBeat);
        
        return CalculateDamageMultiplier(timingDifference);
    }


    public float AdjustDamage(float initialDamage, float multiplier)
        {return initialDamage * multiplier;}


    public float AdjustExpansionRate(float initialExpansionRate, float multiplier)
        {return initialExpansionRate * multiplier;}


    public float AdjustLifetime(float initialLifetime, float multiplier)
        {return initialLifetime * multiplier;}

}
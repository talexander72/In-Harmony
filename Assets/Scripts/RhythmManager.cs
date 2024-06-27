using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RhythmManager : MonoBehaviour 
{
    public AudioSource audioSource;
    
    // damage multiplier variables
    public float lowMultiplier = 0.5f;
    public float mediumMultiplier = 1.0f;
    public float highMultiplier = 3.0f;
    public float perfectTimingWindow = 0.1f;
    
    // beat onset detection variables
    public float sensitivity = 2.0f;
    private float[] samples = new float[1024];
    private float[] spectrum = new float[1024];
    private float averageVolume = 0.0f;
    private float lastAverageVolume = 0.0f;
    public float currentBeat = 0.0f;
    

    private void Start() {
        audioSource.Play();
    }
    

    private void Update()
    {
        // get audio data
        audioSource.GetOutputData(samples, 0);

        // compute average volume
        float sum = 0.0f;
        for (int i = 0; i < samples.Length; i++)
            {sum += samples[i] * samples[i];}
        averageVolume = Mathf.Sqrt(sum / samples.Length);

        // detect beat
        if (averageVolume > lastAverageVolume * sensitivity)
            {OnBeatDetected();}

        lastAverageVolume = averageVolume;
    }
    

    private float GetTime() {
        return audioSource.time; 
    }


    private void OnBeatDetected()
    {
        currentBeat = GetTime();
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
        float timingDifference = Mathf.Abs(attackTime - currentBeat);
        
        return CalculateDamageMultiplier(timingDifference);
    }


    public float AdjustDamage(float initialDamage, float multiplier)
        {return initialDamage * multiplier;}


    public float AdjustExpansionRate(float initialExpansionRate, float multiplier)
        {return initialExpansionRate * multiplier;}


    public float AdjustLifetime(float initialLifetime, float multiplier)
        {return initialLifetime * multiplier;}

}
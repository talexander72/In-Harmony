using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour 
{
    [SerializeField] public float lowMultiplier = 0.5f;
    [SerializeField] public float mediumMultiplier = 1.0f;
    [SerializeField] public float highMultiplier = 3.0f;

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
            if (difference < minDifference) {
                closestBeat = beat;
                minDifference = difference;
            }
        }

        return closestBeat;
    }

    public float CalculateDamageMultiplier(float timingDifference)
    {
        if (timingDifference < Player.perfectTimingWindow) {
            return highMultiplier; // perfect hit
        } else if (timingDifference < Player.perfectTimingWindow * 2) {
            return mediumMultiplier; // good hit
        } else {
            return lowMultiplier; // regular hit
        }
    }


    public float AdjustDamage(float initialDamage)
    {
        float attackTime = GetTime();
        float closestBeat = GetClosestBeatTime(attackTime);
        float timingDifference = Mathf.Abs(attackTime - closestBeat);
        float damageMultiplier = CalculateDamageMultiplier(timingDifference);
        return initialDamage * damageMultiplier;
    }
}
using System;
using UnityEngine;

public class WaveScaling : MonoBehaviour
{
    public static event Action<int> setScaling;

    [SerializeField] private int scalingIncreaseWaveInterval = 10;
    [SerializeField] private int scalingIncreaseWaveRestriction = 10;


    public void UpdateScaling(int waveNumber)
    {
        int wavesPastRestriction = scalingIncreaseWaveRestriction - waveNumber;
        if (wavesPastRestriction < 0) return;
        
        int iterations = (waveNumber / scalingIncreaseWaveRestriction);

        setScaling?.Invoke(iterations);
    }
}

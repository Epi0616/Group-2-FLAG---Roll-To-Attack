using System;
using Unity.VisualScripting;
using UnityEngine;

public class RunTimeStatTracker : MonoBehaviour 
{
    public static RunTimeStatTracker instance;
    public RunTimeStats runTimeStats;

    private void Awake()
    {
        if (instance == null) 
        { 
            instance = this;
            runTimeStats = new RunTimeStats();
            return;
        }

        Destroy(gameObject);
    }
}

[Serializable]
public class RunTimeStats
{
    public int totalDamageDealt { get; set; }
    public float totalTimeSurvived { get; set; }
    public int waveNumber { get; set; }
    public int numberOfAttacks { get; set; }
    public int totalEnemiesKilled { get; set; }

    public RunTimeStats()
    {
        totalDamageDealt = 0;
        totalTimeSurvived = 0f;
        waveNumber = 0;
        numberOfAttacks = 0;
        totalEnemiesKilled = 0;
    }

    public void SetBaseStats(int totalDamageDealt = 0, float totalTimeSurvived = 0, int waveNumber = 0, int numberOfAttacks = 0, int totalEnemiesKilled = 0)
    {
        this.totalDamageDealt = totalDamageDealt;
        this.totalTimeSurvived = totalTimeSurvived;
        this.waveNumber = waveNumber;
        this.numberOfAttacks = numberOfAttacks;
        this.totalEnemiesKilled = totalEnemiesKilled;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class RunTimeStatTracker : MonoBehaviour 
{
    public static GameObject instance;
    public static int totalDamageDealt { get; set; }
    public static float totalTimeSurvived { get; set; }
    public static int waveNumber { get; set; }
    public static int numberOfAttacks { get; set; }
    public static int totalEnemiesKilled { get; set; }
    public static int totalAbilitiesEquipped { get; set; }

    private void Start()
    {
        if (instance == null) 
        { 
            instance = gameObject;
            SetBaseStats();
            return;
        }

        Destroy(gameObject);
    }

    private void SetBaseStats() 
    {
        totalDamageDealt = 0;
        totalTimeSurvived = 0f;
        waveNumber = 0;
        numberOfAttacks = 0;
        totalEnemiesKilled = 0;
        totalAbilitiesEquipped = 0;
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyDirector : MonoBehaviour
{
    public static event Action<List<EnemyTypes>> SpawnWave;
    public static event Action<float> WaveOver;

    [Header("Director Balance Variables")]
    [SerializeField] private float delayBetweenWaves;
    [SerializeField] private int startingBudget;
    [SerializeField] private int budgetIncreasePerWave;
    [SerializeField] private int simpleRaiderCost;
    [SerializeField] private int beholderCost;
    [SerializeField] private int sandGolemCost;

    private List<EnemyTypes> affordableEnemies = new();
    private List<EnemyTypes> generatedEnemies = new();
    private Dictionary<EnemyTypes, int> enemyCosts;

    [Header("Observation Variables not to be Edited")]
    public int enemiesLeftInCurrentWave;
    [SerializeField] private int currentBudget;
    [SerializeField] private int remainingBudget;

    private void OnEnable()
    {
        EnemyStateController.EnemyHasDied += ProcessEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyStateController.EnemyHasDied -= ProcessEnemyDeath;
    }

    void Start()
    {
        enemyCosts = new Dictionary<EnemyTypes, int>()
        {
            { EnemyTypes.SimpleRaider, simpleRaiderCost },
            { EnemyTypes.RangedRaider, beholderCost },
            { EnemyTypes.SandGolem, sandGolemCost },
        };
        currentBudget = startingBudget;
        SelectWave();
       
    }

    private void SelectWave()
    {
        remainingBudget = currentBudget;
        while (remainingBudget > 0)
        {
            affordableEnemies.Clear();
            foreach (var kvp in enemyCosts)
            {
                // Check if there are any affordable enemies
                if (kvp.Value <= remainingBudget)
                {
                    affordableEnemies.Add(kvp.Key);
                }
            }

            if (affordableEnemies.Count == 0)
            {
                break;
            }
            // Select from affordable enemies
            int choice = UnityEngine.Random.Range(0, affordableEnemies.Count);
            EnemyTypes chosenEnemyType = affordableEnemies[choice];
            generatedEnemies.Add(chosenEnemyType);
            remainingBudget -= enemyCosts[chosenEnemyType];
        }
        enemiesLeftInCurrentWave = generatedEnemies.Count;

        WaveOver?.Invoke(delayBetweenWaves);
        StartCoroutine(SpawnWaveDelay());
    }

    IEnumerator SpawnWaveDelay()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        SpawnWave?.Invoke(generatedEnemies);
        
    }

    private void ProcessEnemyDeath()
    {
        //Debug.Log("Enemy Died");
        enemiesLeftInCurrentWave--;
        if (enemiesLeftInCurrentWave <= 0)
        {
            //Debug.Log("New Wave Spawning");
            currentBudget += budgetIncreasePerWave;
            generatedEnemies.Clear();
            SelectWave();
        }
    }
}

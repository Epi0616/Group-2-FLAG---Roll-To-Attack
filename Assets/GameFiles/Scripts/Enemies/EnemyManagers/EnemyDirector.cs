using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyDirector : MonoBehaviour
{
    public static event Action<List<EnemyTypes>> SpawnWave;
    private int budget = 100;
    private int remainingBudget;
    private List<EnemyTypes> affordableEnemies = new();
    private List<EnemyTypes> generatedEnemies = new();
    private Dictionary<EnemyTypes, int> enemyCosts;
    void Start()
    {
        enemyCosts = new Dictionary<EnemyTypes, int>()
        {
            { EnemyTypes.SimpleRaider, 10 },
            { EnemyTypes.RangedRaider, 20 },
            { EnemyTypes.SandGolem, 50 },
        };

        remainingBudget = budget;
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


        StartCoroutine(SpawnWaveDelay());
    }

    IEnumerator SpawnWaveDelay()
    {
        yield return new WaitForSeconds(5f);
        SpawnWave?.Invoke(generatedEnemies);
    }
}

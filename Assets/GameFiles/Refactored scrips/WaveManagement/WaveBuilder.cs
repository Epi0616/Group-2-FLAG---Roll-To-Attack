using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveBuilder : MonoBehaviour 
{
    public static Action<int> EnemiesGenerated;

    [SerializeField] private List<NumberedWave> numberedWaves = new();
    [SerializeField] private List<EntityBlockObj> entityBlocks = new();

    [SerializeField] private int startingBudget;
    [SerializeField] private int currentBudget;
    [SerializeField] private int budgetIncreasePerWave = 0;

    private Dictionary<int, WaveObj> waves = new();
    private List<EntityBlock> entityBlockPool = new();
    private List<EntityBlock> affordableEntities = new();

    private void Start()
    {
        SetUpWavesDictionary();
        entityBlockPool = entityBlocks.Select(c => c.Create()).ToList();
        currentBudget = startingBudget;
    }

    private void SetUpWavesDictionary()
    {
        foreach (NumberedWave numberedWave in numberedWaves)
        {
            waves.Add(numberedWave.waveNumber, numberedWave.waveObj);
        }
    }

    public Wave GetNextWave(int waveIndex)
    {
        Wave currentWave;

        if (waves.ContainsKey(waveIndex))
        {
            currentWave = UnpackWaveObj(waves[waveIndex]);
        }
        else
        {
            currentWave = GenerateWave(waveIndex);
        }

        CountEnemiesInWave(currentWave);
        return currentWave;
    }

    private Wave UnpackWaveObj(WaveObj waveObj)
    {
        Wave wave = waveObj.Create();
        wave.waveGroups = wave.waveGroupObjs.Select(c => c.Create()).ToList();
        for (int i = 0; i < wave.waveGroups.Count; i++)
        {
            wave.waveGroups[i].entityBlocks = wave.waveGroups[i].entityBlockObjs.Select(c => c.Create()).ToList();
            wave.waveGroups[i].conditions.Select(c => c.Clone()).ToList();
        }

        return wave;
    }

    private Wave GenerateWave(int waveIndex)
    {
        List<WaveGroup> chosenWaveGroups = new List<WaveGroup>();

        int remainingBudget = currentBudget;
        while (remainingBudget > 0)
        {
            affordableEntities.Clear();
            foreach (var block in entityBlockPool)
            {
                if (block.cost <= remainingBudget && block.difficultyLevel <= waveIndex)
                {
                    affordableEntities.Add(block);
                }
            }

            if (affordableEntities.Count == 0)
            {
                break;
            }
            // Select from affordable enemies
            int choice = Random.Range(0, affordableEntities.Count);

            WaveGroup currentWaveGroup = new WaveGroup(
                new List<EntityBlock> { affordableEntities[choice] }, 
                new List<BaseWaveCondition> { new TimedWaveCondition((1 - (float)waveIndex/100))}
                );

            chosenWaveGroups.Add(currentWaveGroup);
            remainingBudget -= affordableEntities[choice].cost;
        }

        currentBudget += budgetIncreasePerWave;
        return new Wave(chosenWaveGroups);
    }

    private void CountEnemiesInWave(Wave wave)
    {
        int enemiesInCurrentWave = 0;

        for (int i = 0; i < wave.waveGroups.Count(); i++)
        {
            WaveGroup currentWaveGroup = wave.waveGroups[i];
            float entityBlocksInGroup = currentWaveGroup.entityBlocks.Count();
            for (int j = 0; j < entityBlocksInGroup; j++)
            {
                enemiesInCurrentWave += currentWaveGroup.entityBlocks[j].count;
            }
        }

        EnemiesGenerated?.Invoke(enemiesInCurrentWave);
    }
}

[Serializable]
public struct NumberedWave
{
    public int waveNumber;
    public WaveObj waveObj;
    public NumberedWave(int waveNumber, WaveObj waveObj)
    {
        this.waveNumber = waveNumber;
        this.waveObj = waveObj;
    }
}

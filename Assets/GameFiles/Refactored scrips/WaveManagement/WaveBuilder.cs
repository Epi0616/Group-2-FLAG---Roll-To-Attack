using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveBuilder : MonoBehaviour 
{
    public static Action<int> EnemiesGenerated;

    [SerializeField] private List<NumberedWave> numberedWaves = new();
    [SerializeField] private List<EntityBlockObj> entityBlocks = new();

    [SerializeField] private int startingBudget;
    [SerializeField] private int budgetIncreasePerWave = 0;

    private Dictionary<int, WaveObj> waves = new();
    private List<EntityBlock> entityBlockPool = new();
    private List<EntityBlock> affordableEntities = new();

    private void Start()
    {
        SetUpWavesDictionary();
        entityBlockPool = entityBlocks.Select(c => c.Create()).ToList();
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
            int budget = startingBudget + budgetIncreasePerWave * waveIndex;
            currentWave = GenerateWave(waveIndex, budget);
        }

        CountEnemiesInWave(currentWave);
        return currentWave;
    }

    public Wave UnpackWaveObj(WaveObj waveObj)
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

    public Wave GenerateWave(int waveIndex, int budget)
    {
        List<WaveGroup> chosenWaveGroups = new List<WaveGroup>();

        int remainingBudget = budget;
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

        return new Wave(chosenWaveGroups, WaveType.normal);
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
                enemiesInCurrentWave += CheckCountInBlock(currentWaveGroup.entityBlocks[j]);
            }
        }

        EnemiesGenerated?.Invoke(enemiesInCurrentWave);
    }

    private int CheckCountInBlock(EntityBlock entityBlock)
    {
        if (entityBlock.entity.TryGetComponent<Entity>(out Entity entity))
        {
            if (entity is ISlimeSplit slimeSplit)
            {
                int tally = 0;
                for (int i = 0; i <= slimeSplit.iterationsLeft; i++)
                {
                    tally += (int)Mathf.Pow(slimeSplit.childrenSpawned, i);
                }
                return tally;
            }
        }

        return entityBlock.count;
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

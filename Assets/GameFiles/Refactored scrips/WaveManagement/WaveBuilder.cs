using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveBuilder : MonoBehaviour 
{
    public static Action<int> EnemiesGenerated;

    [SerializeField] private List<WaveObj> waves = new();
    [SerializeField] private List<EntityBlockObj> entityBlocks = new();

    [SerializeField] private int startingBudget;
    [SerializeField] private int currentBudget;

    private List<EntityBlock> entityBlockPool = new();
    private List<EntityBlock> affordableEntities = new();

    private void Start()
    {
        entityBlockPool = entityBlocks.Select(c => c.Create()).ToList();
        currentBudget = startingBudget;
    }

    public Wave GetNextWave(int waveIndex)
    {
        Wave currentWave;

        if (waveIndex < waves.Count)
        {
            Debug.Log("unpacking");
            currentWave = UnpackWaveObj(waves[waveIndex]);
        }
        else
        {
            Debug.Log("generating");
            currentWave = GenerateWave();
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

    private Wave GenerateWave()
    {
        List<WaveGroup> chosenWaveGroups = new List<WaveGroup>();

        int remainingBudget = currentBudget;
        while (remainingBudget > 0)
        {
            affordableEntities.Clear();
            foreach (var block in entityBlockPool)
            {
                if (block.cost <= remainingBudget)
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
                new List<BaseWaveCondition> { new AlwaysTrueWaveCondition()}
                );

            chosenWaveGroups.Add(currentWaveGroup);
            remainingBudget -= affordableEntities[choice].cost;
        }

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

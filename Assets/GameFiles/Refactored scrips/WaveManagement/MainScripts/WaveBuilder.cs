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
    [SerializeField] private int budgetIncreasePerWave = 0;

    [Header("wave spice ;)")]
    [SerializeField] private int regularWaveWeight = 100;
    [SerializeField] private int smartWaveWeight = 50;
    [SerializeField] private int hoardWaveWeight = 10;

    [SerializeField] private List<WavePoolObj> smartWavePoolObjs = new();
    [SerializeField] private List<WavePoolObj> hoardWavePoolObjs = new();

    private List<EntityBlock> entityBlockPool = new();
    private List<WavePool> smartWavePools = new();
    private List<WavePool> hoardWavePools = new();

    private List<EntityBlock> affordableEntities = new();

    private void Start()
    {
        entityBlockPool = entityBlocks.Select(c => c.Create()).ToList();
        smartWavePools = smartWavePoolObjs.Select(c => c.Create()).ToList();
        hoardWavePools = hoardWavePoolObjs.Select(c => c.Create()).ToList();
    }

    public Wave GetNextWave(int waveIndex)
    {
        Wave currentWave;

        if (CheckForIndexedWave(waveIndex, out WaveObj chosenIndexedWaveObj))
        {
            currentWave = UnpackWaveObj(chosenIndexedWaveObj);
        }
        else
        {
            int budget = startingBudget + budgetIncreasePerWave * waveIndex;
            currentWave = GenerateWave(waveIndex, budget);
        }

        CountEnemiesInWave(currentWave);
        return currentWave;
    }

    private bool CheckForIndexedWave(int waveIndex, out WaveObj waveObj)
    {
        waveObj = null;
        List<NumberedWave> potentialWaves = new();

        foreach (NumberedWave numberedWave in numberedWaves)
        {
            if (numberedWave.waveNumber == waveIndex)
            {
                potentialWaves.Add(numberedWave);
                continue;
            }

            int wavesAferNumber = waveIndex - numberedWave.waveNumber;
            if (wavesAferNumber <= 0) continue;
            if (wavesAferNumber % numberedWave.interval != 0) continue;

            potentialWaves.Add(numberedWave);
        }

        if (potentialWaves.Count == 0) return false;

        waveObj = SelectNumberedWaveFromPriority(potentialWaves);
        return true;
    }

    private WaveObj SelectNumberedWaveFromPriority(List<NumberedWave> numberedWaves)
    {
        List<NumberedWave> highestPriorityWaves = new(); //the lower the number the higher the priority
        int highestPriority = int.MaxValue;

        foreach (NumberedWave numberedWave in numberedWaves)
        {
            if (numberedWave.priority < highestPriority)
            {
                highestPriorityWaves.Clear();
                highestPriority = numberedWave.priority;
            }

            if (numberedWave.priority == highestPriority)
            { 
                highestPriorityWaves.Add(numberedWave); //this accounts for the newly selected lowest priority wave as well
            }
        }

        int random = Random.Range(0, highestPriorityWaves.Count);
        return highestPriorityWaves[random].waveObj;
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
        int weightTotal = regularWaveWeight + smartWaveWeight + hoardWaveWeight;
        int weightTally = 0;
        int random = Random.Range(0, weightTotal);

        weightTally += regularWaveWeight;
        if (random < weightTally)
        {
            return GenerateWaveFromBlocks(waveIndex, budget);
        }

        weightTally += smartWaveWeight;
        if (random < weightTally)
        {
            return GenerateWaveFromPool(smartWavePools, waveIndex, budget);
        }

        return GenerateWaveFromPool(hoardWavePools, waveIndex, budget);
    }

    public Wave GenerateWaveFromBlocks(int waveIndex, int budget)
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

    public Wave GenerateWaveFromPool(List<WavePool> pools, int waveIndex, int budget)
    {
        if (!SelectEligableWave(pools, waveIndex, out WavePool selectedPool)) return GenerateWaveFromBlocks(waveIndex, budget);

        List<WaveGroup> chosenWaveGroups = new();
        List<EntityBlock> pooledEntityBlocks = selectedPool.entityBlockObjs.Select(c => c.Create()).ToList();

        int remainingBudget = budget;
        while (remainingBudget > 0)
        {
            affordableEntities.Clear();
            foreach (var block in pooledEntityBlocks)
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
                new List<BaseWaveCondition> { new TimedWaveCondition((1 - (float)waveIndex / 100)) }
                );

            chosenWaveGroups.Add(currentWaveGroup);
            remainingBudget -= affordableEntities[choice].cost;
        }

        return new Wave(chosenWaveGroups, WaveType.normal);
    }

    private bool SelectEligableWave(List<WavePool> wavePools, int waveIndex, out WavePool selectedPool)
    {
        List<WavePool> eligablePools = new();
        selectedPool = null;
        

        foreach (WavePool wavePool in wavePools)
        {
            if (wavePool.waveRestriction <= waveIndex)
            { 
                eligablePools.Add(wavePool);
            }
        }

        if (eligablePools.Count == 0) return false;

        int random = Random.Range(0, eligablePools.Count);
        selectedPool = eligablePools[random];
        return true;
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
    public bool recurring;
    public int interval;
    public int priority;

    public NumberedWave(int waveNumber, WaveObj waveObj, bool recurring = false, int interval = -1, int priority = int.MaxValue)
    {
        this.waveNumber = waveNumber;
        this.waveObj = waveObj;
        this.recurring = recurring;
        this.interval = interval;
        this.priority = priority;
    }
}

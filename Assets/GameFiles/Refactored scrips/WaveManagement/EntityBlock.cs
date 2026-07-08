using System;
using UnityEngine;

[Serializable]
public class EntityBlock : IEntityBlock
{
    [SerializeField] private GameObject Entity;
    [SerializeField] private int DifficultyLevel;
    [SerializeField] private int Count;
    [SerializeField] private float SpawnDelay;
    [SerializeField] private int Cost;

    public EntityBlock(GameObject entity, int difficultyLevel, int count, float spawnDelay, int cost)
    { 
        this.entity = entity;
        this.difficultyLevel = difficultyLevel;
        this.count = count;
        this.spawnDelay = spawnDelay;
        this.cost = cost;
    }

    public GameObject entity { get => Entity; set => Entity = value; }
    public int difficultyLevel { get => DifficultyLevel; set => DifficultyLevel = value; }
    public int count { get => Count; set => Count = value; }
    public float spawnDelay { get => SpawnDelay; set => SpawnDelay = value; }
    public int cost { get => Cost; set => Cost = value; }

    public EntityBlock Clone()
    {
        return new EntityBlock(entity, difficultyLevel, count, spawnDelay, cost);
    }
}

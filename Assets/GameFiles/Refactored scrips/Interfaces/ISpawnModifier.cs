using System;
using UnityEngine;

public interface ISpawnModifier
{
    public SpawnModifier spawnModifier { get; set; }
}

[Serializable]
public enum SpawnModifier
{
    None,
    spawnInGround,
    dragonSpawnInSky,
    SlimeInSky
}
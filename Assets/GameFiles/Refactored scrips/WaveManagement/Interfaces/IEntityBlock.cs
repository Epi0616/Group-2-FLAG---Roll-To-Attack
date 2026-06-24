using UnityEngine;

public interface IEntityBlock
{
    GameObject entity { get; }
    int count { get; }
    float spawnDelay { get; }
    int cost { get; }
}

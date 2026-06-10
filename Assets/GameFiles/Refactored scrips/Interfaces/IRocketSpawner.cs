using UnityEngine;

public interface IRocketSpawner
{
    public GameObject rocketPrefab {  get; set; }
    public int rocketDamage { get; set; }
}

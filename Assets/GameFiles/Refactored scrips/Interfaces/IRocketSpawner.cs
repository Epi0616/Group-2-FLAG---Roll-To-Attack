using UnityEngine;

public interface IRocketSpawner
{
    public GameObject rocketObj {  get; set; }
    public GameObject enhancedRocketObj { get; set; }
    public int rocketDamage { get; set; }
}

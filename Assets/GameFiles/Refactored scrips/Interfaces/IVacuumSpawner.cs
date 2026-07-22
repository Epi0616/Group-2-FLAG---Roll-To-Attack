using UnityEngine;

public interface IVacuumSpawner
{
    public float mineChargeTime { get; set; }

    public GameObject mineObj { get; set; }
    public GameObject enhancedMineObj { get; set; }
}

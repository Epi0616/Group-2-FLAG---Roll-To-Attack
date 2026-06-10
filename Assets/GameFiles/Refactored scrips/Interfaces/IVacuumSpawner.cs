using UnityEngine;

public interface IVacuumSpawner
{
    public float mineChargeTime { get; set; }

    public GameObject minePrefab { get; set; }
}

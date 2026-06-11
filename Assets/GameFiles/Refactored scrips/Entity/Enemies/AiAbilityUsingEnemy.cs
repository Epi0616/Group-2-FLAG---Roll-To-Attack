using System.Collections.Generic;
using UnityEngine;

public class AiAbilityUsingEnemy : BaseAISlamEnemy, IPoisonSpawner, IOrbitSpikeSpawner, IVacuumSpawner, IRocketSpawner
{
    // IPoisonSpawner Interface
    [Header("IPoison Required Properties")]
    [SerializeField] private GameObject PoisonFieldPrefab;
    [SerializeField] private float PoisonFieldLifeTime = 5;
    [SerializeField] private int PoisonFieldDamageTick;
    public GameObject poisonFieldObj { get => PoisonFieldPrefab; set => PoisonFieldPrefab = value; }
    public float fieldLifetime { get => PoisonFieldLifeTime; set => PoisonFieldLifeTime = value; }
    public int fieldTickDamage { get => PoisonFieldDamageTick; set => PoisonFieldDamageTick = value; }

    // IOrbitSpikeSpawner Interface
    private List<BaseOrbitObject> orbitObj = new List<BaseOrbitObject>();
    public List<BaseOrbitObject> orbitObjects { get => orbitObj; set => orbitObj = value; }

    [Header("IOrbitSpike Required Properties")]
    //[SerializeField] private int NumberOfSpikesPerSpawn = 5;
    //public int numSpikesPerSpawn { get => NumberOfSpikesPerSpawn; set => NumberOfSpikesPerSpawn = value; }
    [SerializeField] private float SpikeLifeSpan = 10;
    public float spikeLifeSpan { get => SpikeLifeSpan; set => SpikeLifeSpan = value; }
    [SerializeField] private float SpikeOrbitRadius = 4;
    public float orbitRadius { get => SpikeOrbitRadius; set => SpikeOrbitRadius = value; }
    [SerializeField] private float SpikeOrbitSpeed = 360;
    public float initialOrbitSpeed { get => SpikeOrbitSpeed; set => SpikeOrbitSpeed = value; }
    [SerializeField] private int SpikeDamage;
    public int spikeDamage { get => SpikeDamage; set => SpikeDamage = value; }
    [SerializeField] private GameObject SpikePrefab;
    public GameObject spikePrefab { get => SpikePrefab; set => SpikePrefab = value; }

    [Header("IVacuumSpawner Required Properties")]
    // IVacuumSpawner Interface
    [SerializeField] private float VacuumMineDetonationTime = 5;
    public float mineChargeTime { get => VacuumMineDetonationTime; set => VacuumMineDetonationTime = value; }
    [SerializeField] private GameObject VacuumMinePrefab;
    public GameObject minePrefab { get => VacuumMinePrefab; set => VacuumMinePrefab = value; }

    [Header("IRocketSpawner Required Properties")]
    // IRocketSpawner intercae
    [SerializeField] private int RocketDamage;
    public int rocketDamage { get => RocketDamage; set => RocketDamage = value; }
    [SerializeField] private GameObject RocketPrefab;
    public GameObject rocketPrefab { get => RocketPrefab; set => RocketPrefab = value; }


    // IOrbitSpike Interface Methods
    public void RemoveObjectFromOrbit(BaseOrbitObject obj)
    {
        orbitObjects.Remove(obj);
    }

    public void UpdateOrbitObjectAngles()
    {
        for (int i = 0; i < orbitObjects.Count; i++)
        {
            float angle = i * (360f / orbitObjects.Count);
            orbitObjects[i].UpdateAngle(angle);
        }
    }
}

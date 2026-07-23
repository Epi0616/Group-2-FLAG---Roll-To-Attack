using UnityEngine;

public class NestSpawner : BaseEntityAction, ICreatureSpawner
{
    ICreatureSpawner creatureSpawner;
    [SerializeField] private GameObject CreaturePrefab;
    public GameObject creaturePrefab { get => CreaturePrefab; set => CreaturePrefab = value; }
    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        creatureSpawner = ownerEntity as ICreatureSpawner;
        if (creatureSpawner != null)
        {
            SpawnCreatureAction(creaturePrefab);
        }
    }

    void SpawnCreatureAction(GameObject creature)
    {
        creature = ObjectPoolManager.SpawnObject(CreaturePrefab, ownerEntity.transform.position, Quaternion.identity);
    }
}
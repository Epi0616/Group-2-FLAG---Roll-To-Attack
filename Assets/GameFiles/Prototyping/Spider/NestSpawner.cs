using UnityEngine;

public class NestSpawner : BaseEntityAction
{
    ICreatureSpawner creatureSpawner;
    public NestSpawner() { }
    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        creatureSpawner = ownerEntity.GetComponent<ICreatureSpawner>();
        SpawnCreatureAction(creatureSpawner.creaturePrefab);
    }

    void SpawnCreatureAction(GameObject creature)
    {
        creature = ObjectPoolManager.SpawnObject(creature, ownerEntity.transform.position, Quaternion.identity);
    }

    public override BaseEntityAction Clone()
    {
         return new NestSpawner();
    }
}
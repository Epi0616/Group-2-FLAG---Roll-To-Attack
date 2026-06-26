using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public class EnhancedRocketSpawnSlam : BaseSlamAction , IEnhancedAbility
{
    public int enhancementLevel {  get; set; }

    private IRocketSpawner rocket;

    public int numRockets = 3;
    private float rocketInterval = 0.5f;

    public EnhancedRocketSpawnSlam() { }
    public EnhancedRocketSpawnSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int numRockets, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    { 
        this.enhancementLevel = enhancementLevel;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        rocket = entity as IRocketSpawner;
        if (rocket == null)
        {
            EndAction();
        }
    }

    public override void UpdateAction()
    {
        base.UpdateAction();
    }

    public override void ExtraSlamEffect()
    {
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        if (hitEntity.CompareTag("VacuumMine")) { return; }
        ownerEntity.StartCoroutine(SpawnRockets(hitEntity));
    }


    public override void EndAction()
    {
        base.EndAction();
    }

    private IEnumerator SpawnRockets(Entity hitEntity)
    {
        int count = 0;
        while (count < numRockets && !attackInterrupted && hitEntity != null)
        {
            count++;

            GameObject spawned = ObjectPoolManager.SpawnObject(rocket.enhancedRocketObj, ownerEntity.transform.position, Quaternion.identity);
            spawned.GetComponent<EnhancedSeekingRocket>().Initialize(ownerEntity, hitEntity.gameObject, ownerEntity.transform.position.y, rocket.rocketDamage, enhancementLevel);

            yield return new WaitForSeconds(rocketInterval);
        }
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedRocketSpawnSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, numRockets, enhancementLevel);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

[Serializable]
public class RocketSpawnSlamAction : BaseSlamAction
{
    private IRocketSpawner rocket;

    public int numRockets = 3;
    private float rocketInterval = 0.5f;
    
    public RocketSpawnSlamAction() { }
    public RocketSpawnSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int numRockets) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) { }

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
        base .UpdateAction();
    }

    public override void SpawnSlamStartVFX()
    {
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.slamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange.GetFinalValue(), chargeTime, true);
        Debug.Log(slamRange.GetFinalValue());
    }

    public override void ExtraSlamEffect()
    {        
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        ownerEntity.StartCoroutine(SpawnRockets(hitEntity));
    }


    public override void EndAction()
    {
        base.EndAction();
    }

    private IEnumerator SpawnRockets(Entity hitEntity)
    {
        int count = 0;
        while (count < numRockets && !attackInterrupted)
        {          
            count++;
                
            GameObject spawned = ObjectPoolManager.SpawnObject(rocket.rocketPrefab, ownerEntity.transform.position, Quaternion.identity);
            spawned.GetComponent<SeekingRocket>().Initialize(ownerEntity, hitEntity.gameObject, ownerEntity.transform.position.y, rocket.rocketDamage);

            yield return new WaitForSeconds(rocketInterval);
        }
    }

    public override BaseEntityAction Clone()
    {
        return new RocketSpawnSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, DoesActionPreventMovement, numRockets);
    }
}

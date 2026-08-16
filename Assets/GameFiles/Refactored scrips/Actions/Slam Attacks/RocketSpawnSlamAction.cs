using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

[Serializable]
public class RocketSpawnSlamAction : BaseSlamAction , IUpgradableAbility
{
    private IRocketSpawner rocket;

    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }

    public int numRockets = 3;
    private float rocketInterval = 0.5f;
    
    public RocketSpawnSlamAction() { }
    public RocketSpawnSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int numRockets, ModifiableActionDescriptor result) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) 
    {
        upgradeResult = result;
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
        base .UpdateAction();
    }

    public override void ExtraSlamEffect()
    {        
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        if (hitEntity.CompareTag("StaticEntity") || hitEntity.CompareTag("PhysicsEntity")) { return; }
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);
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
            //ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.VerticalBurst01), ownerEntity.transform.position, Quaternion.Euler(0, 0, 0)).
            //    GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideColour: slamColour, overrideVelocitySpeedMult: 1.5f));
            GameObject spawned = ObjectPoolManager.SpawnObject(rocket.rocketObj, ownerEntity.transform.position, Quaternion.identity);
            spawned.GetComponent<SeekingRocket>().Initialize(ownerEntity, hitEntity.gameObject, ownerEntity.transform.position.y, rocket.rocketDamage);

            yield return new WaitForSeconds(rocketInterval);
        }
    }

    public override BaseEntityAction Clone()
    {
        return new RocketSpawnSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, numRockets, upgradeResult);
    }
}

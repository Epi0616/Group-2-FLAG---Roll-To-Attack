using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnhancedVacuumSlamAction : BaseSlamAction , IEnhancedAbility
{
    private IVacuumSpawner vacuumAccess;
    public int enhancementLevel { get; set; }
    public EnhancedVacuumSlamAction() { }

    public EnhancedVacuumSlamAction(AudioPackage lightImpactNoise, AudioPackage heavyImpactNoise, int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int enhancementLevel) : base(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) 
    {
        this.enhancementLevel = enhancementLevel;
    }



    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        vacuumAccess = entity as IVacuumSpawner;
        if (vacuumAccess == null)
        {
            EndAction();
        }

    }

    public override void SpawnSlamStartVFX()
    {
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.slamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange.GetFinalValue(), chargeTime, false);
    }

    public override void ExtraSlamEffect()
    {
        GameObject vacuumMine = ObjectPoolManager.SpawnObject(vacuumAccess.enhancedMineObj, slamOrigin, Quaternion.identity);
        vacuumMine.GetComponent<NewEVacuumMine>().InitializeMine(ownerEntity, slamRange.GetFinalValue(), vacuumAccess.mineChargeTime * 1.5f, slamColour, enhancementLevel);
    }
    protected override void ApplyHeavyEffectPerEntity(Entity hitEntity)
    {
        float percentage = slamRange.GetFinalValue() / slamRange.GetBaseValue();
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 1.75f * percentage),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true),
            Color.red);
    }
    public override BaseEntityAction Clone()
    {
        return new EnhancedVacuumSlamAction(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, enhancementLevel);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VacuumMineSlamAction : BaseSlamAction , IUpgradableAbility
{
    private IVacuumSpawner vacuumAccess;
    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public VacuumMineSlamAction() { }

    public VacuumMineSlamAction(AudioPackage lightImpactNoise, AudioPackage heavyImpactNoise, int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, ModifiableActionDescriptor result) : base(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        upgradeResult = result;
    }



    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        vacuumAccess  = entity as IVacuumSpawner;
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
        GameObject vacuumMine = ObjectPoolManager.SpawnObject(vacuumAccess.mineObj, slamOrigin, Quaternion.identity);
        vacuumMine.GetComponent<NewVacuumMine>().InitializeMine(ownerEntity, slamRange.GetFinalValue(), vacuumAccess.mineChargeTime, slamColour);
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
        return new VacuumMineSlamAction(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, upgradeResult);
    }
}

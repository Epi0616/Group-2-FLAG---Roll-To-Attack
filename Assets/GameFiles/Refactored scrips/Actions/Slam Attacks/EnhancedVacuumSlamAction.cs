using UnityEngine;
using System;

[Serializable]
public class EnhancedVacuumSlamAction : BaseSlamAction , IEnhancedAbility
{
    private IVacuumSpawner vacuumAccess;
    public int enhancementLevel { get; set; }
    public EnhancedVacuumSlamAction() { }

    public EnhancedVacuumSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) 
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
        vacuumMine.GetComponent<EnhancedVacuumMine>().Initialize(ownerEntity, slamRange.GetFinalValue(), vacuumAccess.mineChargeTime * 3f, enhancementLevel);
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedVacuumSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, DoesActionPreventMovement, enhancementLevel);
    }
}

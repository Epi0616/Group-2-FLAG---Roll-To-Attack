using UnityEngine;
using System;

[Serializable]
public class VacuumMineSlamAction : BaseSlamAction
{
    private IVacuumSpawner vacuumAccess;

    public VacuumMineSlamAction() { }

    public VacuumMineSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) { }



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
        vacuumMine.GetComponent<VacuumMine>().Initialize(ownerEntity, slamRange.GetFinalValue(), vacuumAccess.mineChargeTime);
    }

    public override BaseEntityAction Clone()
    {
        return new VacuumMineSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, DoesActionPreventMovement);
    }
}

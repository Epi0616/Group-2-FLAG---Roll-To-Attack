using UnityEngine;
using System;

[Serializable]
public class VacuumMineSlamAction : BaseSlamAction , IUpgradableAbility
{
    private IVacuumSpawner vacuumAccess;
    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public VacuumMineSlamAction() { }

    public VacuumMineSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, ModifiableActionDescriptor result) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
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
        vacuumMine.GetComponent<NewVacuumMine>().Initialize(ownerEntity, slamRange.GetFinalValue(), vacuumAccess.mineChargeTime);
    }

    public override BaseEntityAction Clone()
    {
        return new VacuumMineSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, upgradeResult);
    }
}

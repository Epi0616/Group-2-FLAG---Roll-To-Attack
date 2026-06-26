using UnityEngine;
using System;

[Serializable]
public class PoisonSlamAction : BaseSlamAction , IUpgradableAbility
{
    protected IPoisonSpawner poisonAccess;

    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }

    public PoisonSlamAction() { }

    public PoisonSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) { }
   
    

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        poisonAccess = entity as IPoisonSpawner;
        if (poisonAccess == null)
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
        GameObject poisonField = ObjectPoolManager.SpawnObject(poisonAccess.poisonFieldObj, slamOrigin, Quaternion.identity);
        poisonField.GetComponent<PoisonField>().Initialize(ownerEntity, slamRange.GetFinalValue(), poisonAccess.fieldLifetime, poisonAccess.fieldTickDamage, slamColour);
    }

    protected override void ApplyHeavyEffect(Entity hitEntity)
    {

    }

    public override BaseEntityAction Clone()
    {
        return new PoisonSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}

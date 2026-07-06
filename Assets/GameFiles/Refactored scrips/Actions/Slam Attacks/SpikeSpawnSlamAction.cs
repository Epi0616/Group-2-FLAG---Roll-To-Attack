using UnityEngine;
using System;

[Serializable]
public class SpikeSpawnSlamAction : BaseSlamAction , IUpgradableAbility
{
    private IOrbitSpikeSpawner spike;

    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public SpikeSpawnSlamAction() { }
    public SpikeSpawnSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, ModifiableActionDescriptor result) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) 
    {
        upgradeResult = result;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        spike = entity as IOrbitSpikeSpawner;
        if (spike == null)
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
        for (int i = 0; i < 5; i++)
        {
            //GameObject spike = Instantiate(playerSpike);
            GameObject newObj = ObjectPoolManager.SpawnObject(spike.spikePrefab, new Vector3(0, -100, 0), Quaternion.identity);
            BaseOrbitObject orbitObject = newObj.GetComponent<BaseOrbitObject>();
            spike.orbitObjects.Add(orbitObject);
            spike.UpdateOrbitObjectAngles();
            orbitObject.Initialize(ownerEntity, ownerEntity.gameObject, spike.orbitRadius, spike.initialOrbitSpeed, spike.spikeDamage, spike.spikeLifeSpan);
        }
        spike.RefreshSpikeAge();
    }

    public override BaseEntityAction Clone()
    {
        return new SpikeSpawnSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, upgradeResult);
    }
}

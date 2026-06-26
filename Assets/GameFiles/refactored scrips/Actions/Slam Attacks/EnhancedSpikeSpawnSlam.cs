using UnityEngine;
using System;

[Serializable]
public class EnhancedSpikeSpawnSlam : BaseSlamAction , IEnhancedAbility
{
    private IOrbitSpikeSpawner spike;

    public int enhancementLevel {  get; set; }

    public EnhancedSpikeSpawnSlam() { }
    public EnhancedSpikeSpawnSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.enhancementLevel = enhancementLevel;
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
            GameObject newObj = ObjectPoolManager.SpawnObject(spike.enhancedSpikePrefab, new Vector3(0, -100, 0), Quaternion.identity);
            EnhancedOrbitingSpike orbitObject = newObj.GetComponent<EnhancedOrbitingSpike>();
            spike.orbitObjects.Add(orbitObject);
            spike.UpdateOrbitObjectAngles();
            orbitObject.Initialize(ownerEntity, ownerEntity.gameObject, spike.orbitRadius, spike.initialOrbitSpeed, spike.spikeDamage, spike.spikeLifeSpan, enhancementLevel);
        }
        spike.RefreshSpikeAge();
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedSpikeSpawnSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, enhancementLevel);
    }
}

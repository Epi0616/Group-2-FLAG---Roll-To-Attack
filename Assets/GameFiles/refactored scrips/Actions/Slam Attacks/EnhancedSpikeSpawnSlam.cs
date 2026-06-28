using System;
using System.Security.Principal;
using UnityEngine;

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
            Vector3 offset = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, UnityEngine.Random.Range(-0.5f, 0.5f));
            Vector3 spawnPos = ownerEntity.transform.position + offset;
            GameObject newObj = ObjectPoolManager.SpawnObject(spike.enhancedSpikePrefab, spawnPos, Quaternion.identity);
            EnhancedOrbitingSpike orbitObject = newObj.GetComponent<EnhancedOrbitingSpike>();
            spike.orbitObjects.Add(orbitObject);
            spike.UpdateOrbitObjectAngles();
            orbitObject.Initialize(ownerEntity, ownerEntity.gameObject, spike.orbitRadius, spike.initialOrbitSpeed, spike.spikeDamage, spike.spikeLifeSpan, enhancementLevel);
        }
        spike.RefreshSpikeAge();
        
    }

    protected override void ApplyExtraHeavyEffect()
    {
        spike.EjectEnhancedSpikes();
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedSpikeSpawnSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, enhancementLevel);
    }
}

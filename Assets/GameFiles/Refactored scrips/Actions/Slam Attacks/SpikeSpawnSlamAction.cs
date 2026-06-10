using UnityEngine;
using System;

[Serializable]
public class SpikeSpawnSlamAction : BaseSlamAction
{
    private IOrbitSpikeSpawner spike;

    public int numSpikes = 5;
    public SpikeSpawnSlamAction() { }
    public SpikeSpawnSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int numSpikes) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent) { }

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
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.SlamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange, chargeTime, true);
    }

    public override void ExtraSlamEffect()
    {
        for (int i = 0; i < numSpikes; i++)
        {
            //GameObject spike = Instantiate(playerSpike);
            GameObject newObj = ObjectPoolManager.SpawnObject(spike.spikePrefab, new Vector3(0, -100, 0), Quaternion.identity);
            BaseOrbitObject orbitObject = newObj.GetComponent<BaseOrbitObject>();
            spike.orbitObjects.Add(orbitObject);
            spike.UpdateOrbitObjectAngles();
            orbitObject.Initialize(ownerEntity, ownerEntity.gameObject, spike.orbitRadius, spike.initialOrbitSpeed, spike.spikeDamage, spike.spikeLifeSpan);
        }
    }

    public override BaseEntityAction Clone()
    {
        return new SpikeSpawnSlamAction(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesActionPreventMovement, numSpikes);
    }
}

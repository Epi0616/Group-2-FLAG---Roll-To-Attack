using System;
using UnityEngine;

[Serializable]
public class BolderThrowAction : BaseSlamAction
{
    protected override void SetupSlam()
    {
        slamVariablesAccess = ownerEntity as ISlamActionRequirements;
        chargeUpTimer = 0;
        chargeComplete = false;
        attackInterrupted = false;

        slamOrigin = ownerEntity.target.transform.position;

        IBoulderThrow boulderThrow = ownerEntity as IBoulderThrow;
        GameObject boulder = ObjectPoolManager.SpawnObject(boulderThrow.boulderObj, ownerEntity.transform.position, Quaternion.identity);
        boulder.GetComponent<ThrowableBoulder>().HandlePathToTarget(slamOrigin, chargeTime);

        SpawnSlamStartVFX();
    }
}

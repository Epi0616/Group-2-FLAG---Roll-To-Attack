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
        SpawnSlamStartVFX();
    }
}

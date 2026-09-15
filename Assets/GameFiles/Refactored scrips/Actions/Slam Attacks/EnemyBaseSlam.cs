using UnityEngine;
using System;

[Serializable]
public class EnemyBaseSlam : BaseSlamAction
{
    public EnemyBaseSlam() { }
    public EnemyBaseSlam(AudioPackage lightImpactNoise, AudioPackage heavyImpactNoise, int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent) : base(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {

    }
    protected override void SetupSlam()
    {
        slamVariablesAccess = ownerEntity as ISlamActionRequirements;
        chargeUpTimer = 0;
        chargeComplete = false;
        attackInterrupted = false;

        //slamImpactField = slamVariablesAccess.SlamImpactField;
        // Debug.Log("SLAM STRTED");
        //RaycastHit hit;
        slamOrigin = ownerEntity.transform.position + (ownerEntity.transform.forward * slamPositionOffset.z) + (ownerEntity.transform.right * slamPositionOffset.x + (Vector3.up * slamPositionOffset.y));
        //if (Physics.Raycast(slamOrigin, Vector3.down,out hit, 10))
        //{
        //    slamOrigin.y = hit.point.y;
        //}


        // + ownerEntity.transform.TransformPoint(slamVariablesAccess.slamPositionOffset);
        //EnemyAttackImpactField field = slamVariablesAccess.SPAWNTHING(slamVariablesAccess.DebugSlamObj, slamOrigin).GetComponent<EnemyAttackImpactField>();
        SpawnSlamStartVFX();
    }
    
    public override void SpawnSlamCompleteVFX()
    {
        
    }

    public override BaseEntityAction Clone()
    {
        return new EnemyBaseSlam(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}

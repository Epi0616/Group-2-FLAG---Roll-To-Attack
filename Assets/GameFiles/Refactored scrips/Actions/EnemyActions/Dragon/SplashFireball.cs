using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class SplashFireballAction : ArcingProjectileThrowAction
{
    public SplashFireballAction() { }
    public SplashFireballAction(bool preventsMovement, Vector3 offset, int slamDamage, Color slamColor, float chargeTime, Stat slamRange) : base(preventsMovement, offset, slamDamage, slamColor, chargeTime, slamRange)
    { 
        this.preventsMovement = preventsMovement;
        this.offset = offset;
        this.slamDamage = slamDamage;
        this.chargeTime = chargeTime;
        this.slamRange = slamRange;
    }

    protected override IEnumerator Action()
    {
        projectile = ObjectPoolManager.SpawnObject(arcingProjectile.arcingProjectileObj, ownerEntity.transform.position, Quaternion.identity).GetComponent<ArcingProjectile>();
        if (projectile is IDecalShadowCast shadow)
        {
            shadow.currentShadowDecal = ObjectPoolManager.SpawnObject(shadow.shadowDecalPrefab, projectile.transform.position, new Quaternion(1, 0, 0, 1)).GetComponent<ShadowDecal>();
            shadow.currentShadowDecal.SetupProjector(new Vector2(5, 5), new Quaternion(1, 0, 0, 1), new Vector3(0, 0, 0), true, false, projectile.gameObject);
        }
        projectile.HandlePathToTarget(ownerEntity, PickRandomAreaOnNavMesh(), 3, slamDamage, slamColour, slamRange.GetFinalValue());

        yield return new WaitForSeconds(1);

        actionRoutine = null;
        projectile = null;
        EndAction();
    }

    private Vector3 PickRandomAreaOnNavMesh()
    {
        Vector3 centrePosition = new Vector3(2.1f, 0, 15);
        Vector3 finalPosition;

        float radius = 25;

        float x = Random.Range(-radius, radius);
        float z = Random.Range(-radius, radius);

        finalPosition = new Vector3(centrePosition.x + x, 0, centrePosition.z + z);

        if (NavMesh.SamplePosition(finalPosition, out NavMeshHit hit, 5, -1))
        {
            return hit.position;
        }

        return ownerEntity.target.transform.position;
    }

    public override BaseEntityAction Clone()
    {
        return new SplashFireballAction(preventsMovement, offset, slamDamage, SlamColor, chargeTime, slamRange);
    }
}

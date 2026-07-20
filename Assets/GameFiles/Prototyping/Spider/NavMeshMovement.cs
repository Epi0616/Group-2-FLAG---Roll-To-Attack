using System;
using UnityEngine;

[Serializable]
public class NavMeshMovement1 : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval = 1f;
    private float intervalTimer = 0;
    public NavMeshMovement1() { }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        enemyBodySystem = ownerEntity.bodySystem as EnemyBodySystem;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;

        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 1);
        }
    }

    public override void UpdateMovement()
    {
        if (ownerEntity == null) return;
        if (aiInterfaceAccess.agent == null) { Debug.LogError("NO AGENT LOL"); }

        if (moveable.canMove == false) { EndMovement(); return; }


        intervalTimer += Time.deltaTime;
        if (intervalTimer > setDestinationInterval)
        {
            aiInterfaceAccess.agent.SetDestination(new Vector3(ownerEntity.target.transform.position.x + ownerEntity.target.transform.position.z, 0, ownerEntity.target.transform.position.z + ownerEntity.target.transform.position.x));
            intervalTimer = 0;
        }
    }

    public override void InterruptMovement()
    {
        EndMovement();
    }

    public override void EndMovement()
    {
       // Debug.Log("Movement Ended");
        aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position);
    }
    public override BaseEntityMovement Clone()
    {
        return new NavMeshMovement1();
    }

}

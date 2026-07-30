using System;
using UnityEngine;


[Serializable]
public class NavMeshMovementHook : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private IActionable action;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval;
    private float intervalTimer = 0;
    public bool hitKnockedOut = false;
    public NavMeshMovementHook() { }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        action = ownerEntity as IActionable;
        setDestinationInterval = UnityEngine.Random.Range(1f, 4f);
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

        aiInterfaceAccess.agent.SetDestination(ownerEntity.target.transform.position);
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
        return new NavMeshMovementHook();
    }

}

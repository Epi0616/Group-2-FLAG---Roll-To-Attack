using System;
using UnityEngine;

[Serializable]
public class NavMeshMovement : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval = 0.15f;
    private float intervalTimer = 0;
    public NavMeshMovement() { }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        enemyBodySystem = ownerEntity.bodySystem as EnemyBodySystem;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;

        if (ownerEntity is IAnimated animated)
        {
            Debug.Log("base movement setting anim");
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main);
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
            
            aiInterfaceAccess.agent.SetDestination(ownerEntity.target.transform.position);
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
        return new NavMeshMovement();
    }

}

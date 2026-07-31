using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class NavMeshMovementHook : BaseEntityMovement
{
    
    private INavAgent aiInterfaceAccess;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval = 0.15f;
    private float intervalTimer = 0;
    private Transform target;
    public NavMeshMovementHook() { }

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
        //intervalTimer += Time.deltaTime;
        //if (intervalTimer > setDestinationInterval)
        //{
        //    aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position + ownerEntity.transform.forward * 100f * Time.deltaTime);
        //    intervalTimer = 0;
        //}

        //ownerEntity.transform.position += ownerEntity.transform.forward * 10f * Time.deltaTime;
        aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position + ownerEntity.transform.forward * 100f * Time.deltaTime);
        NavMeshHit hit;
        if (NavMesh.Raycast(ownerEntity.transform.position, target.position, out hit, NavMesh.AllAreas))
        { 
            ownerEntity.transform.Rotate(Vector3.up, 65f * Time.deltaTime);
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
        return new NavMeshMovementHook();
    }

}

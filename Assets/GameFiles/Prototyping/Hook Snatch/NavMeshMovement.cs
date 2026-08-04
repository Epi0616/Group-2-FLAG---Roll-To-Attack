using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class NavMeshMovementHook : BaseEntityMovement
{
    
    private INavAgent aiInterfaceAccess;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval = 5f;
    private float intervalTimer = 0;
    private float y;
    private bool touched;
    public NavMeshMovementHook() { }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        enemyBodySystem = ownerEntity.bodySystem as EnemyBodySystem;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;
        y = ownerEntity.transform.rotation.y;

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
            aiInterfaceAccess.agent.SetDestination(ownerEntity.target.transform.position);
        }
        if (intervalTimer < setDestinationInterval)
        {
            aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position + 100f * Time.deltaTime * ownerEntity.transform.forward);
        }
        if (intervalTimer > setDestinationInterval + 1)
        {
            intervalTimer = 0;
        }
        aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position + 100f * Time.deltaTime * ownerEntity.transform.forward);
        NavMeshHit hit;
        if (NavMesh.FindClosestEdge(ownerEntity.transform.position, out hit, NavMesh.AllAreas))
        {
            float distanceToEdge = hit.distance;
            if (distanceToEdge <= 0.00001f)
            {
                ownerEntity.transform.rotation = Quaternion.Euler(ownerEntity.transform.rotation.x, y += 120, ownerEntity.transform.rotation.x);
            }
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

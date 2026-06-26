using System;
using UnityEngine;


[Serializable]
public class NavMeshMovement : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval = 0.15f;
    private float intervalTimer = 0;
    private bool isAnimated;
    public NavMeshMovement() { }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        enemyBodySystem = ownerEntity.bodySystem as EnemyBodySystem;
        isAnimated = true;
        if (enemyBodySystem == null)
        {
            isAnimated = false;
        }
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;
    }

    public override void UpdateMovement()
    {        
        if (ownerEntity == null) return;
        if (aiInterfaceAccess.agent == null) { Debug.LogError("NO AGENT LOL"); }

        if (isAnimated) { enemyBodySystem.UpdateAnimatorSpeedParamter(aiInterfaceAccess.agent.velocity.magnitude / aiInterfaceAccess.agent.speed); }

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
        if (isAnimated) { enemyBodySystem.UpdateAnimatorSpeedParamter(0f); }
    }
    public override BaseEntityMovement Clone()
    {
        return new NavMeshMovement();
    }

}

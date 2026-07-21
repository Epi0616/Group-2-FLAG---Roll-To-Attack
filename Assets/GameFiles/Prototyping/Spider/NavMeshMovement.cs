using System;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

[Serializable]
public class NavMeshMovement1 : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private IActionable action;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval;
    private float intervalTimer = 0;
    public NavMeshMovement1() { }

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

        intervalTimer += Time.deltaTime;
        if (intervalTimer > setDestinationInterval)
        {
            if(setDestinationInterval <= 2)
            {
                MoveAroundObjectClockwise();
            }
            if(setDestinationInterval > 2f)
            {
                MoveAroundObjectAnti();
            }
            intervalTimer = 0;
        }

        if(aiInterfaceAccess.agent.velocity.magnitude <= 0)
        {
            action.canAct = true;
        }
        if (aiInterfaceAccess.agent.velocity.magnitude > 0)
        {
            action.canAct = false;
        } 
            
    }

    public void MoveAroundObjectAnti()
    {
        Vector3 offsetObj = ownerEntity.target.transform.position - ownerEntity.transform.position;
        Vector3 dir = Quaternion.Euler(0, 50, 0) * offsetObj;
        dir += ownerEntity.transform.position;
        aiInterfaceAccess.agent.SetDestination(dir);
    }
    public void MoveAroundObjectClockwise()
    {
        Vector3 offsetObj = ownerEntity.target.transform.position - ownerEntity.transform.position;
        Vector3 dir = Quaternion.Euler(0, -50, 0) * offsetObj;
        dir += ownerEntity.transform.position;
        aiInterfaceAccess.agent.SetDestination(dir);
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

using System;
using UnityEngine;

[Serializable]
public class NavMeshMovementHook : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval = 10f;
    private float intervalTimer = 0;
    private bool updatePosition = true;
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

        Debug.Log(updatePosition);
        //intervalTimer += Time.deltaTime;
        //if (intervalTimer > setDestinationInterval)
        //{
        //    aiInterfaceAccess.agent.SetDestination(ownerEntity.target.transform.position);
        //    intervalTimer = 0;
        //}
        if (updatePosition)
        {
            updatePosition = false;
        }
        if (!updatePosition)
        {
            aiInterfaceAccess.agent.SetDestination(new Vector3(-57f, 0.714470029f, 10f));
            aiInterfaceAccess.agent.updateRotation = true;
            
        }

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
        return new NavMeshMovementHook();
    }

}

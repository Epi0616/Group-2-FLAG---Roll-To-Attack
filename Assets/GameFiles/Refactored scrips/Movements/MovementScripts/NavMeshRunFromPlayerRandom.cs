using System.Collections.Generic;
using UnityEngine;

public class NavMeshRunFromPlayerRandom : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private ITarget target;
    private EnemyBodySystem enemyBodySystem;

    private float initialSpeed = 0f;

    Vector3 DirectionOfApproach;

    public NavMeshRunFromPlayerRandom() { }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        enemyBodySystem = ownerEntity.bodySystem as EnemyBodySystem;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;

        if (ownerEntity.target.TryGetComponent<ITarget>(out ITarget target))
        {
            this.target = target;
        }

        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 1);
        }

        DirectionOfApproach = (ownerEntity.transform.position - ownerEntity.target.transform.position).normalized;

        ActiveStatusEffect speedIncreaseEffect = new(new MovementSpeedStatus(0.5f), new List<BaseCondition> { new DistanceCondition(true, 30) }, true);
        ownerEntity.statusSystem.OnRecieveEffect(speedIncreaseEffect);
    }

    public override void UpdateMovement()
    {
        if (ownerEntity == null) return;
        if (aiInterfaceAccess.agent == null) { Debug.LogError("NO AGENT LOL"); }

        if (moveable.canMove == false) { EndMovement(); return; }


        //intervalTimer += Time.deltaTime;
        //if (intervalTimer > setDestinationInterval)
        //{
        //    aiInterfaceAccess.agent.SetDestination(FindDestinationAwayFromTarget());
        //    intervalTimer = 0;
        //}
    }

    private Vector3 FindDestinationAwayFromTarget()
    {
        Vector3 chosenPoint = Vector3.zero;

        return chosenPoint;
    }

    public override void InterruptMovement()
    {
        EndMovement();
    }

    public override void EndMovement()
    {
        // Debug.Log("Movement Ended");
        aiInterfaceAccess.agent.speed = initialSpeed;
        aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position);
    }
    public override BaseEntityMovement Clone()
    {
        return new NavMeshEscapeMovement();
    }
}

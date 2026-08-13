using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class NavMeshEscapeMovement : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private ITarget target;
    private float setDestinationInterval = 0.15f;
    private float intervalTimer = 0;

    private float initialSpeed = 0f;

    public NavMeshEscapeMovement() { }

    public override void StartMovement(Entity ownerEntity)
    {
        Debug.Log("starting escape movement");
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;

        if (ownerEntity.target.TryGetComponent<ITarget>(out ITarget target))
        { 
            this.target = target;
        }

        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main);
        }

        ActiveStatusEffect speedIncreaseEffect = new (new MovementSpeedStatus(0.35f), new List<BaseCondition>{ new DistanceCondition(true, 30) }, true);
        ownerEntity.statusSystem.OnRecieveEffect(speedIncreaseEffect);
    }

    public override void UpdateMovement()
    {
        if (ownerEntity == null) return;
        if (aiInterfaceAccess.agent == null) { Debug.LogError("NO AGENT LOL"); }

        if (moveable.canMove == false) { EndMovement(); return; }


        intervalTimer += Time.deltaTime;
        if (intervalTimer > setDestinationInterval)
        {
            aiInterfaceAccess.agent.SetDestination(FindDestinationAwayFromTarget());
            intervalTimer = 0;
        }
    }

    private Vector3 FindDestinationAwayFromTarget()
    {
        List<Vector3> potentialPoints = target.perimeterPoints;
        float smallestDistance = float.MaxValue;
        Vector3 chosenPoint = Vector3.zero;

        for (int i = 0; i < potentialPoints.Count; i++)
        {
            float distanceToPoint = (potentialPoints[i] - ownerEntity.transform.position).magnitude;

            if (distanceToPoint < smallestDistance)
            {
                chosenPoint = potentialPoints[i];
                smallestDistance = distanceToPoint;

            }
        }

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

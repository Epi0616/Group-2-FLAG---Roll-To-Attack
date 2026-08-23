using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class OrbitCentreMovement : OrbitalIntervalMovement
{
    [SerializeField] protected Vector3 centrePoint;
    [SerializeField] protected float speedUp;

    public OrbitCentreMovement() { }
    public OrbitCentreMovement(float radiusMin, float radiusMax, float angleMin, float angleMax, float intervalMin, float intervalMax, float reverseChancePercentage, Vector3 centrePoint, float speedUp)
    { 
        this.radiusBounds.min = radiusMin;
        this.radiusBounds.max = radiusMax;
        this.angleBounds.min = angleMin;
        this.angleBounds.max = angleMax;
        this.intervalBounds.min = intervalMin;
        this.intervalBounds.max = intervalMax;
        this.reverseChancePercentage = reverseChancePercentage;
        this.centrePoint = centrePoint;
        this.speedUp = speedUp;
    }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) { Debug.LogError("ownerEntity is not of type INavAgent"); return; }
        this.navAgent = navAgent;

        if (ownerEntity is not IAnimated animated) { Debug.LogError("ownerEntity is not of type IAnimated"); return; }
        this.animated = animated;

        navAgent.agent.updateRotation = false;
        
        ActiveStatusEffect speedIncreaseEffect = new(new MovementSpeedStatus(speedUp), new List<BaseCondition> { new DistanceCondition(true, 30) }, true);
        ownerEntity.statusSystem.OnRecieveEffect(speedIncreaseEffect);
        //CheckForReverseMovement();
        PickDestination();
    }

    public override void UpdateMovement()
    {
        ReverseMovementBasedOnTargetPos();

        timer -= Time.deltaTime;
        if (timer > 0)
        {
            return;
        }

        PickDestination();
        SetTimer();
    }

    public override void PickDestination()
    {
        float angle = Random.Range(angleBounds.min, angleBounds.max) * reverse;
        float radius = Random.Range(radiusBounds.min, radiusBounds.max);

        Vector3 directionToTarget = (ownerEntity.transform.position - centrePoint).normalized;
        directionToTarget.y = 0;
        Vector3 rotatedVector = Quaternion.Euler(0, angle, 0) * directionToTarget;

        Vector3 desiredPosition = centrePoint + (rotatedVector * radius);

        if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, 10, -1))
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main);
            navAgent.agent.SetDestination(desiredPosition);
        }
    }

    private void ReverseMovementBasedOnTargetPos()
    {
        Vector3 directionToTarget = (ownerEntity.target.transform.position - centrePoint).normalized;
        Vector3 directionToSelf = (ownerEntity.transform.position - centrePoint).normalized;

        Vector2 flatDirectionToTarget = new Vector2(directionToTarget.z, directionToTarget.x);
        Vector2 flatDirectionToSelf = new Vector2(directionToSelf.z, directionToSelf.x);

        float result = Cross2D(flatDirectionToSelf, flatDirectionToTarget);
        if (result >= 0)
        {
            reverse = -1;
            return;
        }
        reverse = 1;
    }

    private static float Cross2D(Vector2 a, Vector2 b)
    { 
        return a.x*b.y-b.x*a.y;
    }

    public override BaseEntityMovement Clone()
    {
        return new OrbitCentreMovement(radiusBounds.min, radiusBounds.max, angleBounds.min, angleBounds.max, intervalBounds.min, intervalBounds.max, reverseChancePercentage, centrePoint, speedUp);
    }
}

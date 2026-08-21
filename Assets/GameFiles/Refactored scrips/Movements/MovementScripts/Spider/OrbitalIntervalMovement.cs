using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class OrbitalIntervalMovement : BaseEntityMovement
{
    [SerializeField] protected rangePair radiusBounds = new rangePair(20,35);
    [SerializeField] protected rangePair angleBounds = new rangePair (30,50);
    [SerializeField] protected rangePair intervalBounds = new rangePair(1, 2);
    [Tooltip("percentage chance between 0-1")]
    [SerializeField] protected float reverseChancePercentage = 0.2f;

    protected IAnimated animated;
    protected INavAgent navAgent;
    protected int reverse = 1;
    protected float timer = 0;

    public OrbitalIntervalMovement() { }

    public OrbitalIntervalMovement(float radiusMin, float radiusMax, float angleMin, float angleMax, float intervalMin, float intervalMax, float reverseChancePercentage)
    { 
        radiusBounds.min = radiusMin;
        radiusBounds.max = radiusMax;
        angleBounds.min = angleMin;
        angleBounds.max = angleMax;
        intervalBounds.min = intervalMin;
        intervalBounds.max = intervalMax;
        this.reverseChancePercentage = reverseChancePercentage;
    }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) { Debug.LogError("ownerEntity is not of type INavAgent"); return; }
        this.navAgent = navAgent;

        if (ownerEntity is not IAnimated animated) { Debug.LogError("ownerEntity is not of type IAnimated"); return; }
        this.animated = animated;

        navAgent.agent.updateRotation = false;
        PickDestination();
    }

    public override void UpdateMovement()
    {
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            return;
        }

        PickDestination();
        CheckForReverseMovement();
        SetTimer();
    }

    public virtual void PickDestination()
    {
        float angle = Random.Range(angleBounds.min, angleBounds.max) * reverse;
        float radius = Random.Range(radiusBounds.min, radiusBounds.max);

        Vector3 directionToTarget = (ownerEntity.transform.position - ownerEntity.target.transform.position).normalized;
        Vector3 rotatedVector = Quaternion.Euler(0, angle, 0) * directionToTarget;

        Vector3 desiredPosition = ownerEntity.target.transform.position + (rotatedVector * radius);

        if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, 5, -1))
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main);
            navAgent.agent.SetDestination(desiredPosition);
        }
    }

    protected virtual void CheckForReverseMovement()
    {
        if (Random.Range(0f, 1f) < reverseChancePercentage)
        {
            reverse *= -1;
        }
    }

    protected virtual void SetTimer()
    {
        timer = Random.Range(intervalBounds.min, intervalBounds.max);
    }

    public override void InterruptMovement()
    {
        EndMovement();
    }

    public override void EndMovement()
    {
        navAgent.agent.SetDestination(ownerEntity.transform.position);
    }

    public override BaseEntityMovement Clone()
    {
        return new OrbitalIntervalMovement(radiusBounds.min, radiusBounds.max, angleBounds.min, angleBounds.max, intervalBounds.min, intervalBounds.max, reverseChancePercentage);
    }
}

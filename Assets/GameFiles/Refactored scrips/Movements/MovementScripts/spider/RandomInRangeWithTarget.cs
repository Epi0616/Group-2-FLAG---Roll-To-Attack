using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class RandomInRangeWithTarget : RandomInRangeNavMesh
{
    [SerializeField] private float angleVariance = 5f;
    public RandomInRangeWithTarget() : base() { }
    public RandomInRangeWithTarget(float rangeMin, float rangeMax, float interval, float intervalVariance, float angleVariance) : base(rangeMin, rangeMax, interval, intervalVariance)
    {
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
        this.interval = interval;
        this.intervalVariance = intervalVariance;
        this.angleVariance = angleVariance;
    }

    protected override void PickDestination()
    {
        bool destinationFound = false;

        while (!destinationFound)
        {
            float randomAngle = FindAngleToTargetWithVariance();
            float randomRadius = Random.Range(rangeMin, rangeMax);

            float angleInRad = randomAngle;
            Vector3 pointToCheck = ownerEntity.transform.position;
            pointToCheck.x += randomRadius * Mathf.Cos(angleInRad);
            pointToCheck.y = 1.7f; //hard coded to be the height of the arena for now, will adjust at some point...
            pointToCheck.z += randomRadius * Mathf.Sin(angleInRad);

            NavMeshHit hit;
            NavMesh.SamplePosition(pointToCheck, out hit, 2, NavMesh.AllAreas);

            if (hit.hit)
            {
                destinationFound = true;
                navAgent.agent.SetDestination(hit.position);
            }
        }
    }

    protected float FindAngleToTargetWithVariance()
    {
        Vector3 directionToTarget = ownerEntity.target.transform.position - ownerEntity.transform.position;
        directionToTarget.Normalize();

        float angle = Mathf.Atan2(directionToTarget.z, directionToTarget.x);
        float variance = Random.Range(-angleVariance, angleVariance);
        angle += variance * Mathf.Deg2Rad;

        return angle;
    }

    protected override void SetTimer()
    {
        base.SetTimer();
    }

    public override BaseEntityMovement Clone()
    {
        return new RandomInRangeWithTarget(rangeMin, rangeMax, interval, intervalVariance, angleVariance);
    }
}

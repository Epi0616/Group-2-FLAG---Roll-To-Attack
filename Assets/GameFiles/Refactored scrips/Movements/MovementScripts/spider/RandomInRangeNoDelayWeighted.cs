using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class RandomInRangeNoDelayWeighted : BaseEntityMovement
{
    [SerializeField] protected float rangeMin, rangeMax;
    [SerializeField] protected float angleVariance;
    [SerializeField] protected float chancePercentForTargettedMovement;

    protected INavAgent navAgent;
    private Vector3 destination;

    public RandomInRangeNoDelayWeighted() { }
    public RandomInRangeNoDelayWeighted(float rangeMin, float rangeMax, float angleVariance, float chancePercentForTargettedMovement)
    {
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
        this.angleVariance = angleVariance;
        this.chancePercentForTargettedMovement = chancePercentForTargettedMovement;
    }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) { Debug.Log("owner entity is not of type INavAgent"); return; }
        this.navAgent = navAgent;

        PickDestination();
    }

    public override void UpdateMovement()
    {
        if (navAgent.agent.pathPending) return;
        if (navAgent.agent.remainingDistance > navAgent.agent.stoppingDistance) return;
        if (navAgent.agent.velocity.sqrMagnitude > 0) return;

        PickDestination();
    }

    protected void PickDestination()
    {
        float random = Random.Range(0f, 1f);

        if (random <= chancePercentForTargettedMovement)
        {
            PickDestinationTargeted();
            return;
        }
        PickDestinationRandom();
    }

    protected virtual void PickDestinationRandom()
    {
        bool destinationFound = false;

        while (!destinationFound)
        {
            float randomAngle = Random.Range(0, 360);
            float randomRadius = Random.Range(rangeMin, rangeMax);

            float angleInRad = randomAngle * Mathf.Deg2Rad;
            Vector3 pointToCheck = ownerEntity.transform.position;
            pointToCheck.x += randomRadius * Mathf.Cos(angleInRad);
            pointToCheck.y = 1.7f; //hard coded to be the height of the arena for now, will adjust at some point...
            pointToCheck.z += randomRadius * Mathf.Sin(angleInRad);

            NavMeshHit hit;
            NavMesh.SamplePosition(pointToCheck, out hit, 2, NavMesh.AllAreas);

            if (hit.hit)
            {
                destinationFound = true;
                destination = hit.position;
                navAgent.agent.SetDestination(destination);
            }
        }
    }

    protected void PickDestinationTargeted()
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

    public override void InterruptMovement()
    {
    }

    public override void EndMovement()
    {

    }

    public override BaseEntityMovement Clone()
    {
        return new RandomInRangeNoDelayWeighted(rangeMin, rangeMax, angleVariance, chancePercentForTargettedMovement);
    }
}

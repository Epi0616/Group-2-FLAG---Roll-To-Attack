using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class RandomInRangeNoDelay : BaseEntityMovement
{
    [SerializeField] protected float rangeMin, rangeMax;

    protected INavAgent navAgent;
    private Vector3 destination;

    public RandomInRangeNoDelay() { }
    public RandomInRangeNoDelay(float rangeMin, float rangeMax)
    {
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
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

    protected virtual void PickDestination()
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

    public override void InterruptMovement()
    {
    }

    public override void EndMovement()
    {

    }

    public override BaseEntityMovement Clone()
    {
        return new RandomInRangeNoDelay(rangeMin, rangeMax);
    }
}

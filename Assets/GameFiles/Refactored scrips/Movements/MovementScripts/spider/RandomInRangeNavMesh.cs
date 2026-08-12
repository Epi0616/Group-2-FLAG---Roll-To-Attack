using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class RandomInRangeNavMesh : BaseEntityMovement
{
    [SerializeField] protected float rangeMin, rangeMax;
    [SerializeField] protected float interval = 2f;
    [SerializeField] protected float intervalVariance = 1f;

    protected INavAgent navAgent;
    protected float timer;

    public RandomInRangeNavMesh() { }
    public RandomInRangeNavMesh(float rangeMin, float rangeMax, float interval, float intervalVariance)
    { 
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
        this.interval = interval;
        this.intervalVariance = intervalVariance;
    }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) { Debug.Log("owner entity is not of type INavAgent"); return; }
        this.navAgent = navAgent;

        SetTimer();
    }

    public override void UpdateMovement()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PickDestination();
            SetTimer();
        }
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
                navAgent.agent.SetDestination(hit.position);
            }
        }
    }

    protected virtual void SetTimer()
    { 
        float variance = Random.Range(-intervalVariance, intervalVariance);
        timer = interval + variance;
    }

    public override void InterruptMovement()
    {
    }

    public override void EndMovement()
    {
        
    }

    public override BaseEntityMovement Clone()
    {
        return new RandomInRangeNavMesh(rangeMin, rangeMax, interval, intervalVariance);
    }
}

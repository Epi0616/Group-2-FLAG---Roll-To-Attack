using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class WalkTowardsPlayer : BaseEntityAction
{
    [SerializeField] protected float rangeMin, rangeMax;
    [SerializeField] private float angleVariance = 5f;

    private INavAgent navAgent;
    private Coroutine actionRoutine;
    private bool tempPreventsMovement;

    public WalkTowardsPlayer() { }
    public WalkTowardsPlayer(bool preventsMovement, float rangeMin, float rangeMax, float angleVariance)
    {
        tempPreventsMovement = preventsMovement;
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
        this.angleVariance = angleVariance;
    }

    public override void StartAction(Entity ownerEntity)
    {
        Debug.Log("starting walkTowardsPlayer");
        base.StartAction(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) { Debug.LogError("entity is not of type INavAgent"); return; }
        this.navAgent = navAgent;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        while (navAgent.agent.pathPending)
        {
            yield return null;
        }

        while (navAgent.agent.remainingDistance > navAgent.agent.stoppingDistance)
        {
            yield return null;
        }

        preventsMovement = tempPreventsMovement; //this and above is to finish the current movement before starting the walk towards player

        if (!PickDestination())
        {
            Debug.LogWarning("no valid destination for spooderman :(");
            actionRoutine = null;
            EndAction();
            yield break;
        }

        while (navAgent.agent.pathPending)
        {
            yield return null;
        }

        while (navAgent.agent.remainingDistance > navAgent.agent.stoppingDistance)
        {
            yield return null;
        }

        actionRoutine = null;
        EndAction();
    }

    protected bool PickDestination()
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
            navAgent.agent.SetDestination(hit.position);
            return true;
        }

        return false;
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

    public override void InterruptAction()
    {
        if (actionRoutine != null)
        {
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }

        EndAction();
    }

    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new WalkTowardsPlayer(preventsMovement, rangeMin, rangeMax, angleVariance);
    }
}

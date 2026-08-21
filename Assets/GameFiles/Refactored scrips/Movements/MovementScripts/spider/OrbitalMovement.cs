using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class OrbitalMovement : BaseEntityMovement
{
    [SerializeField] private rangePair radiusBounds = new rangePair(20,35);
    [SerializeField] private rangePair angleBounds = new rangePair (30,50);
    [SerializeField] private rangePair delayBounds = new rangePair (0, 0.5f);
    [Tooltip("percentage change between 0-1")]
    [SerializeField] private float reverseChancePercentage = 0.2f;

    private INavAgent navAgent;
    private int reverse = 1;
    private float timer = 0;

    public OrbitalMovement() { }

    public OrbitalMovement(float radiusMin, float radiusMax, float angleMin, float angleMax, float delayMin, float delayMax, float reverseChancePercentage)
    { 
        radiusBounds.min = radiusMin;
        radiusBounds.max = radiusMax;
        angleBounds.min = angleMin;
        angleBounds.max = angleMax;
        this.reverseChancePercentage = reverseChancePercentage;
        delayBounds.min = delayMin;
        delayBounds.max = delayMax;
    }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) { Debug.LogError("ownerEntity is not of type INavAgent"); return; }
        this.navAgent = navAgent;
        navAgent.agent.updateRotation = false;
    }

    public override void UpdateMovement()
    {
        if (navAgent.agent.pathPending) return;
        if (navAgent.agent.remainingDistance > navAgent.agent.stoppingDistance) return;
        if (navAgent.agent.velocity.sqrMagnitude > 0) return;

        timer -= Time.deltaTime;
        if (timer > 0)
        {
            return;
        }

        PickDestination();
        CheckForReverseMovement();
        SetTimer();
    }

    public void PickDestination()
    {
        float angle = Random.Range(angleBounds.min, angleBounds.max) * reverse;
        float radius = Random.Range(radiusBounds.min, radiusBounds.max);

        Vector3 directionToTarget = (ownerEntity.transform.position - ownerEntity.target.transform.position).normalized;
        Vector3 rotatedVector = Quaternion.Euler(0, angle, 0) * directionToTarget;

        Vector3 desiredPosition = ownerEntity.target.transform.position + (rotatedVector * radius);
        navAgent.agent.SetDestination(desiredPosition);
    }

    private void CheckForReverseMovement()
    {
        if (Random.Range(0f, 1f) < reverseChancePercentage)
        {
            reverse *= -1;
        }
    }

    private void SetTimer()
    {
        timer = Random.Range(delayBounds.min, delayBounds.max);
    }

    public override void InterruptMovement()
    {
        
    }

    public override void EndMovement()
    {

    }

    public override BaseEntityMovement Clone()
    {
        return new OrbitalMovement(radiusBounds.min, radiusBounds.max, angleBounds.min, angleBounds.max, delayBounds.min, delayBounds.max, reverseChancePercentage);
    }
}

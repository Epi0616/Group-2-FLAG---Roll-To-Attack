using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class OrbitalEscapeMovement : BaseEntityMovement
{
    [SerializeField] private rangePair radiusBounds = new rangePair(20,35);
    [SerializeField] private rangePair angleBounds = new rangePair (30,50);
    [SerializeField] private rangePair intervalBounds = new rangePair(1, 2);
    [Tooltip("percentage chance between 0-1")]
    [SerializeField] private float reverseChancePercentage = 0.2f;

    private INavAgent navAgent;
    private int reverse = 1;
    private float timer = 0;

    public OrbitalEscapeMovement() { }

    public OrbitalEscapeMovement(float radiusMin, float radiusMax, float angleMin, float angleMax, float intervalMin, float intervalMax, float reverseChancePercentage)
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
        Debug.Log("escape movement");
        base.StartMovement(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) { Debug.LogError("ownerEntity is not of type INavAgent"); return; }
        this.navAgent = navAgent;
        navAgent.agent.updateRotation = false;
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
        timer = Random.Range(intervalBounds.min, intervalBounds.max);
    }

    public override void InterruptMovement()
    {
        
    }

    public override void EndMovement()
    {

    }

    public override BaseEntityMovement Clone()
    {
        return new OrbitalEscapeMovement(radiusBounds.min, radiusBounds.max, angleBounds.min, angleBounds.max, intervalBounds.min, intervalBounds.max, reverseChancePercentage);
    }
}

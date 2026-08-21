using System;
using UnityEngine;

[Serializable]
public class DistanceFromPointWithDelay : BaseCondition
{
    private Entity entity;
    public float distanceThreshold;
    [SerializeField] private Vector3 point;
    [SerializeField] private float delay;

    private bool isConditionMet = true;
    private bool activated = false;
    private float timer;

    public DistanceFromPointWithDelay() { }

    public DistanceFromPointWithDelay(bool inverse, float distanceThreshold, Vector3 point, float delay)
    {
        this.inverse = inverse;
        this.distanceThreshold = distanceThreshold;
        this.point = point;
        this.delay = delay;

        isConditionMet = true;
        activated = false;
        timer = delay;
    }
    public override void Initialize(Entity entity)
    {
        this.entity = entity;
    }
    public override void ConditionUpdate()
    {
        if (!activated)
        {
            isConditionMet = (point - entity.transform.position).magnitude < distanceThreshold;
            if (!isConditionMet)
            {
                activated = true;
            }
        }
        else 
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Debug.Log("delay over");
                activated = false;
                timer = delay;
            }
        }
    }
    public override void ResetCondition()
    {
        isConditionMet = true;
        activated = false;
        timer = delay;
    }
    public override bool IsConditionMet()
    {
        return inverse ? !isConditionMet : isConditionMet;
    }
    public override BaseCondition Clone()
    {
        return new DistanceFromPointWithDelay(inverse, distanceThreshold, point, delay);
    }
}

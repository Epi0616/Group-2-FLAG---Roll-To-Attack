using System;
using UnityEngine;

[Serializable]
public class DistanceConditionWithHold : DistanceCondition
{
    [SerializeField] private float holdTime = 4f;

    private bool activated = false;
    private bool isConditionMet = false;

    private float timer = 0;

    public DistanceConditionWithHold() { }
    public DistanceConditionWithHold(bool inverse, float distanceThreshold, float holdTime)
    { 
        this.inverse = inverse;
        this.distanceThreshold = distanceThreshold;
        this.holdTime = holdTime;
    }

    public override void ConditionUpdate()
    {
        if (!activated)
        {
            isConditionMet = (ownerEntity.target.transform.position - ownerEntity.transform.position).magnitude < distanceThreshold;
            if (isConditionMet)
            {
                activated = true;
            }
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Debug.Log("delay over");
            activated = false;
            timer = holdTime;
        }
    }

    public override bool IsConditionMet()
    {
        return inverse? !isConditionMet : isConditionMet;
    }

    public override BaseCondition Clone()
    {
        return new DistanceConditionWithHold(inverse, distanceThreshold, holdTime);
    }
}

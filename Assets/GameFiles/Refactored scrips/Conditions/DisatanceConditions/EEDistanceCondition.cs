using System;
using UnityEngine;

[Serializable]
public class EEDistanceCondition : EntraceExitBaseCondition
{
    [SerializeField] private float entranceDistanceThreshold;
    [SerializeField] private float exitDistanceThreshold;

    EEDistanceCondition() { }
    EEDistanceCondition(float entranceDistanceThreshold, float exitDistanceThreshold)
    { 
        this.entranceDistanceThreshold = entranceDistanceThreshold;
        this.exitDistanceThreshold = exitDistanceThreshold;
    }

    protected override bool CheckEntranceCondition()
    {
        bool isMet = (ownerEntity.target.transform.position - ownerEntity.transform.position).magnitude < entranceDistanceThreshold;

        if (isMet)
        { 
            entered = true;
            return true;
        }
        return false;
    }

    protected override bool CheckExitConditon()
    {
        bool isMet = (ownerEntity.target.transform.position - ownerEntity.transform.position).magnitude < exitDistanceThreshold;

        if (isMet)
        {
            return true;
        }

        entered = false;
        return false;
    }

    public override BaseCondition Clone()
    {
        return new EEDistanceCondition(entranceDistanceThreshold, exitDistanceThreshold);
    }
}

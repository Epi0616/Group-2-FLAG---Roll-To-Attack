using System;
using UnityEngine;

[Serializable]
public class MilestoneInfo
{
    public Milestone milestone { get; set; }
    public GameObject milestoneObj;
    public float healthMilestone;

    public MilestoneInfo() { }
    public MilestoneInfo(GameObject milestoneObj, float healthMilestone)
    { 
        this.milestoneObj = milestoneObj;
        this.healthMilestone = healthMilestone; //percent 0-1
    }

    public void ActivateMilestone()
    {
        milestone.Activate();
    }

    public MilestoneInfo Clone()
    {
        return new MilestoneInfo(milestoneObj, healthMilestone);
    }
}

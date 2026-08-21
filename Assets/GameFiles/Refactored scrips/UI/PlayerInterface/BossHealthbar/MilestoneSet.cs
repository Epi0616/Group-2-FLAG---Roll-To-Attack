using UnityEngine;

[CreateAssetMenu(fileName = "MilestoneSet", menuName = "BossMilestones/MilestoneSet")]
public class MilestoneDescriptor : ScriptableObject
{
    public MilestoneInfo milestone;

    public MilestoneInfo Create()
    { 
        return milestone.Clone();
    }
}

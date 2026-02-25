using UnityEngine;

public class AbilityBay : AbilityDropZoneParent
{
    private void Start()
    {
        objectLimit = 5;
    }
    protected override void FormatChildren()
    {
        base.FormatChildren();
        displayCapacity(draggableObjects.Count);
    }
}

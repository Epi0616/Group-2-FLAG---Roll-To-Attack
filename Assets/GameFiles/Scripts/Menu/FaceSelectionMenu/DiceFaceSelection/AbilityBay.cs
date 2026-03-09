using UnityEngine;
using System.Collections.Generic;

public class AbilityBay : AbilityDropZoneParent
{
    protected override void Awake()
    {
        base.Awake();
        objectLimit = 5;
    }
    protected override void FormatChildren()
    {
        base.FormatChildren();
        displayCapacity(draggableObjects.Count);
    }
    public List<DraggableObject> GetChildren()
    {
        return draggableObjects;
    }
}

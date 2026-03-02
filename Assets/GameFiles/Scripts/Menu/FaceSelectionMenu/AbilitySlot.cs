using System.Linq;
using UnityEngine;

public class AbilitySlot : AbilityDropZoneParent
{
    protected override void Awake()
    {
        base.Awake();
        objectLimit = 1;
    }

    public DraggableObject GetChild()
    {
        if (draggableObjects.Count == 0) { return null; }
        return draggableObjects[0];
    }
}

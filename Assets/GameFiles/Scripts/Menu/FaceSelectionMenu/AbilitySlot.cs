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
        if (!draggableObjects[0]) { return null; }
        return draggableObjects[0];
    }
}

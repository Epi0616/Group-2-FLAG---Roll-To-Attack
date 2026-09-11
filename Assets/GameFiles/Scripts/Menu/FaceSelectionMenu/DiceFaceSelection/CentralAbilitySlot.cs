using UnityEngine;

public class CentralAbilitySlot : AbilitySlot
{
    private bool initalized = false;

    public override bool TryAddChild(DraggableObject newObject)
    {
        if (initalized) return false;
        
        base.TryAddChild(newObject);
        initalized = true;
        return true;
    }

    public void ResetSlot()
    {
        initalized = false;
    }
}

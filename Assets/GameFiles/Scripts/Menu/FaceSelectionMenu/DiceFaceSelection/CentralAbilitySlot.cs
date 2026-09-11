using UnityEngine;

public class CentralAbilitySlot : AbilitySlot
{
    private bool initalized = false;

    public override bool TryAddChild(DraggableObject newObject)
    {
        if (draggableObjects.Contains(newObject)) { FormatChildren(); return false; }

        if (draggableObjects.Count > 0)
        {
            SwapAbilitiesWithUpgrade(newObject);
            //SwapAbilitiesWithUpgrade(newObject);
            return true;
        }

        if (initalized) { return false; }

        draggableObjects.Add(newObject);
        newObject.SetCurrentParent(this);
        FormatChildren();
        if (diceSlot)
        {
            GlowTo(1, baseColor, 0.2f, false);
        }

        initalized = true;
        return true;
    }

    public void ResetSlot()
    {
        initalized = false;
    }
}

using UnityEngine;
using System;

public class EquippableActionHolder
{
    public ModifiableActionDescriptor actionDescriptor;
    public ConditionalAction actionInstance;
    public int EnhancementLevel = 1;

    public EquippableActionHolder() { }
    public EquippableActionHolder(ModifiableActionDescriptor actionDescriptor, int enhancementLevel)
    {
        if (actionDescriptor == null) { Debug.LogWarning("Descriptor null"); }
        this.actionDescriptor = actionDescriptor;
        actionInstance = actionDescriptor.action.Create();
        UpdateEnhancementLevel(enhancementLevel);
    }

    public EquippableActionHolder Clone()
    {
        return new EquippableActionHolder(actionDescriptor, EnhancementLevel);
    }

    public void UpdateEnhancementLevel(int newLevel)
    {
        if ( actionInstance.action is IEnhancedAbility temp)
        {
            EnhancementLevel = newLevel;
            temp.enhancementLevel = EnhancementLevel;
        }
    }
}

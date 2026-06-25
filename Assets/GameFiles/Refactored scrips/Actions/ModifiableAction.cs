using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class ModifiableAction
{
    // functionality
    [SerializeField]
    public ConditionalActionDescriptor conditionalActionDescriptor;
    public ConditionalAction conditionalAction;
    public int weighting = 100;
    public int enhancementLevel = 0;

    //description
    public LocalizedString actionName;
    public LocalizedString actionDescription;
    public Sprite sprite;

    public ModifiableAction(ConditionalActionDescriptor conditionalActionDescriptor, int weighting, int enhancementLevel, LocalizedString actionName, LocalizedString actionDescription, Sprite sprite)
    { 
        this.conditionalActionDescriptor = conditionalActionDescriptor;
        this.weighting = weighting;
        this.enhancementLevel = enhancementLevel;
        this.actionName = actionName;
        this.actionDescription = actionDescription;
        this.sprite = sprite;
    }
    public ModifiableAction(ConditionalAction conditionalAction, int weighting, int enhancementLevel, LocalizedString actionName, LocalizedString actionDescription, Sprite sprite)
    {
        this.conditionalAction = conditionalAction;
        this.weighting = weighting;
        this.enhancementLevel = enhancementLevel;
        this.actionName = actionName;
        this.actionDescription = actionDescription;
        this.sprite = sprite;
    }

    public ModifiableAction Clone()
    {
        ModifiableAction newAction = new ModifiableAction(conditionalActionDescriptor.Create(), weighting, enhancementLevel, actionName, actionDescription, sprite);
        newAction.conditionalActionDescriptor = conditionalActionDescriptor;
        return newAction;
    }

    public void UpdateEnhancementLevel(int newLevel)
    {
        enhancementLevel = newLevel;
        if (conditionalAction.action is IEnhancedAbility temp)
        {
            temp.enhancementLevel = enhancementLevel;
        }
    }
}

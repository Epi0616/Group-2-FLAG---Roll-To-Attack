using System;
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
    public AbilityType abilityType;

    public ModifiableAction(ConditionalActionDescriptor conditionalActionDescriptor, int weighting, int enhancementLevel, LocalizedString actionName, LocalizedString actionDescription, Sprite sprite, AbilityType abilityType)
    { 
        this.conditionalActionDescriptor = conditionalActionDescriptor;
        this.conditionalAction = conditionalActionDescriptor.Create();
        this.weighting = weighting;
        this.enhancementLevel = enhancementLevel;
        this.actionName = actionName;
        this.actionDescription = actionDescription;
        this.sprite = sprite;
        this.abilityType = abilityType;
        UpdateEnhancementLevel(enhancementLevel);
    }
    public ModifiableAction(ConditionalAction conditionalAction, int weighting, int enhancementLevel, LocalizedString actionName, LocalizedString actionDescription, Sprite sprite, AbilityType abilityType)
    {
        this.conditionalAction = conditionalAction;
        this.weighting = weighting;
        this.enhancementLevel = enhancementLevel;
        this.actionName = actionName;
        this.actionDescription = actionDescription;
        this.sprite = sprite;
        this.abilityType = abilityType;
        UpdateEnhancementLevel(enhancementLevel);
    }

    public ModifiableAction Clone()
    {
        ModifiableAction newAction = new ModifiableAction(conditionalActionDescriptor.Create(), weighting, enhancementLevel, actionName, actionDescription, sprite, abilityType);
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

public enum AbilityType
{
    Basic,
    Freeze,
    Poison,
    Spike,
    Knockback,
    Rocket,
    Slow,
    Weaken,
    Vacuum,
}

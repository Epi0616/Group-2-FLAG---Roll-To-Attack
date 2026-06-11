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
    public int weighting = 100;

    //description
    public LocalizedString actionName;
    public LocalizedString actionDescription;
    public Sprite sprite;

    public ModifiableAction(ConditionalActionDescriptor conditionalActionDescriptor, int weighting, LocalizedString actionName, LocalizedString actionDescription, Sprite sprite)
    { 
        this.conditionalActionDescriptor = conditionalActionDescriptor;
        this.weighting = weighting;
        this.actionName = actionName;
        this.actionDescription = actionDescription;
        this.sprite = sprite;
    }
}

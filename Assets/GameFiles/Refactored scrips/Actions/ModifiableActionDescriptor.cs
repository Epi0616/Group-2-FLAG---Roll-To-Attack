using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class ModifiableActionDescriptor : ScriptableObject
{
    // functionality
    public ConditionalAction action;
    public int weighting = 100;

    //description
    public LocalizedString actionName;
    public LocalizedString actionDescription;
    public Sprite sprite;

    public ModifiableAction Create()
    {
        return new ModifiableAction(action, weighting, actionName, actionDescription, sprite);
    }
}

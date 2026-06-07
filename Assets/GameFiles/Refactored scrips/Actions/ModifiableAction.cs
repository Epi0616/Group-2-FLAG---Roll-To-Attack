using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifiableAction
{
    [SerializeField]
    public ConditionalAction conditionalAction;

    public int weighting = 100;

    public ModifiableAction(ConditionalAction conditionalAction, int weighting)
    { 
        this.conditionalAction = conditionalAction;
        this.weighting = weighting;
    }
}

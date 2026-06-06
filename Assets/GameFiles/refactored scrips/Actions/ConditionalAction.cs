using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalAction
{
    [SerializeReference, SubclassSelector]
    public IAction action;
    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public bool singleUse = false, triggered = false;
    //public ConditionalAction() { }

    public ConditionalAction(IAction action, List<BaseCondition> conditions, bool singleUse)
    {
        this.action = action;
        this.conditions = conditions;
        this.singleUse = singleUse;
    }
}

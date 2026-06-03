using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalAction
{
    [SerializeReference, SubclassSelector]
    public IAction action;
    [SerializeReference, SubclassSelector]
    public List<ICondition> conditions;

    //public ConditionalAction() { }

    public ConditionalAction(IAction action, List<ICondition> conditions)
    {
        this.action = action;
        this.conditions = conditions;
    }
}

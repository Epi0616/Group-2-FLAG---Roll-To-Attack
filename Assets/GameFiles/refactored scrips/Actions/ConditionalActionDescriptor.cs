using System.Collections.Generic;
using UnityEngine;

public class ConditionalActionDescriptor : ScriptableObject
{
    public int variable = 1;

    [SerializeReference, SubclassSelector]
    public IAction action;

    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public bool singleUse = false;

    public ConditionalAction Create()
    {
        return new ConditionalAction(action, conditions, singleUse);
    }
}

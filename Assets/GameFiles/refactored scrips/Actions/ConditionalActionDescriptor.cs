using System.Collections.Generic;
using UnityEngine;

public class ConditionalActionDescriptor : ScriptableObject
{
    public int variable = 1;

    [SerializeReference, SubclassSelector]
    public IAction action;

    [SerializeReference, SubclassSelector]
    public List<ICondition> conditions;

    public ConditionalAction Create(List<ICondition> condiitons, IAction actions)
    {
        return new ConditionalAction(action, conditions);
    }
}

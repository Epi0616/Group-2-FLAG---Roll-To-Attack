using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionalActionDescriptor : ScriptableObject
{
    public int variable = 1;

    [SerializeReference, SubclassSelector]
    public BaseEntityAction action;

    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public bool allConditionsRequired = false;

    public bool singleUse = false;
    public bool exclusive = true;
    public int priority = 0;

    public ConditionalAction Create()
    {
        return new ConditionalAction(action.Clone(), conditions.Select(c => c.Clone()).ToList(), singleUse, exclusive, priority, allConditionsRequired);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ConditionalAction
{
    [SerializeReference, SubclassSelector]
    public BaseEntityAction action;
    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public bool allConditionsRequired = false;
    public bool singleUse = false, triggered = false;
    public bool exclusive = true;
    public bool interruptOnDeath = true;
    public int priority = 0;

    public ConditionalAction() { }

    public ConditionalAction(BaseEntityAction action, List<BaseCondition> conditions, bool singleUse, bool exclusive, bool interruptOnDeath, int priority, bool allConditionsRequired)
    {
        this.action = action;
        this.conditions = conditions;
        this.singleUse = singleUse;
        this.exclusive = exclusive;
        this.interruptOnDeath = interruptOnDeath;
        this.priority = priority;
        this.allConditionsRequired = allConditionsRequired;
    }

    public ConditionalAction Clone()
    {
        return new ConditionalAction(action.Clone(), conditions.Select(c => c.Clone()).ToList(), singleUse, exclusive, interruptOnDeath, priority, allConditionsRequired);
    }

    public void UpdateConditionsAll()
    {
        foreach (BaseCondition condition in conditions)
        {
            condition.ConditionUpdate();
        }
    }

    public void ResetConditionsAll()
    {
        foreach (BaseCondition condition in conditions)
        {
            condition.ResetCondition();
        }
    }

}

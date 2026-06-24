using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalMovement
{
    [SerializeReference, SubclassSelector]
    public BaseEntityMovement movement;
    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;
    public bool allConditionsRequired;
    public bool exclusive = true;
    public int priority = 0;

    //public ConditionalAction() { }

    public ConditionalMovement(BaseEntityMovement movement, List<BaseCondition> conditions, bool exclusive, int priority, bool allConditionsRequired)
    {
        this.movement = movement;
        this.conditions = conditions;
        this.exclusive = exclusive;
        this.priority = priority;
        this.allConditionsRequired = allConditionsRequired;
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

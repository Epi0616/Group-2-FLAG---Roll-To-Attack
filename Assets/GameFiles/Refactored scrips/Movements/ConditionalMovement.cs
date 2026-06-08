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

    //public ConditionalAction() { }

    public ConditionalMovement(BaseEntityMovement movement, List<BaseCondition> conditions)
    {
        this.movement = movement;
        this.conditions = conditions;
    }
}

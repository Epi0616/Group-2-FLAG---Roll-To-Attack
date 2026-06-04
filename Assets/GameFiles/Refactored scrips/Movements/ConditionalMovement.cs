using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalMovement
{
    [SerializeReference, SubclassSelector]
    public IMovement movement;
    [SerializeReference, SubclassSelector]
    public List<ICondition> conditions;

    //public ConditionalAction() { }

    public ConditionalMovement(IMovement movement, List<ICondition> conditions)
    {
        this.movement = movement;
        this.conditions = conditions;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class ConditionalMovementDescriptor : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    public BaseEntityMovement movement;

    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public bool allConditionsRequired = false;
    public bool exclusive = true;
    public int priority = 0;

    public ConditionalMovement Create()
    {      
        return new ConditionalMovement(movement.Clone(), conditions.Select(c => c.Clone()).ToList(), exclusive, priority, allConditionsRequired);
    }
}

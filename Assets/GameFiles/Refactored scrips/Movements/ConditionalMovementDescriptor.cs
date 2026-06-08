using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConditionalMovementDescriptor : ScriptableObject
{
    public int variable = 1;

    [SerializeReference, SubclassSelector]
    public BaseEntityMovement movement;

    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public ConditionalMovement Create()
    {
        return new ConditionalMovement(movement.Clone(), conditions.Select(c => c.Clone()).ToList());
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ConditionalMovementDescriptor : ScriptableObject
{
    public int variable = 1;

    [SerializeReference, SubclassSelector]
    public IMovement movement;

    [SerializeReference, SubclassSelector]
    public List<ICondition> conditions;

    public ConditionalMovement Create()
    {
        return new ConditionalMovement(movement, conditions);
    }
}

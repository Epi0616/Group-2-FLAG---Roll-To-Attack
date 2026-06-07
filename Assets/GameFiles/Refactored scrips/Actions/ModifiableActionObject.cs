using System.Collections.Generic;
using UnityEngine;

public class ModifiableActionDescriptor : ScriptableObject
{
    public ConditionalAction action;
    public int weighting = 100;

    public ModifiableAction Create()
    {
        return new ModifiableAction(action, weighting);
    }
}

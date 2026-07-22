using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionalActionDescriptor : ScriptableObject
{
    public ConditionalAction conditionalAction;

    public ConditionalAction Create()
    {
        return conditionalAction.Clone();
    }
}

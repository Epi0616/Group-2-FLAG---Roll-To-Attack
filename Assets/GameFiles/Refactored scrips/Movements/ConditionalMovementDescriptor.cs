using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class ConditionalMovementDescriptor : ScriptableObject
{
    public ConditionalMovement conditionalMovement;

    public ConditionalMovement Create()
    {
        return conditionalMovement.Clone();
    }
}

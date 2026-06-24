using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    List<ConditionalMovementDescriptor> conditionalMovementDescriptors { get; set; }
    List<ConditionalMovement> conditionalMovements { get; set; }
    public MovementController movementController { get; set; }
    public bool canMove { get; set; }
    public Stat movementSpeed { get; set; }

    public void CheckForCanMove();
    void UnpackConditionalMovements();
}

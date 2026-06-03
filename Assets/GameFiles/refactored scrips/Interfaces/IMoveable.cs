using UnityEngine;

public interface IMoveable
{
    public MovementController movementController { get; set; }
    public bool canMove { get; set; }
    public Stat movementSpeed { get; set; }

    public void CheckForCanMove();
}

using UnityEngine;
using System;

public class Player : Entity, IMoveable, IActionable, IGrounded
{
    //IGrounded Interface properties
    public bool isGrounded { get; set; }

    //IMoveable Interface properties
    public bool canMove { get; set; }
    public Stat movementSpeed { get; set; }
    public MovementController movementController { get; set; }

    //IActionable Interface properties
    public ActionController actionController { get; set; }
    public bool canAct { get; set; }

    private void Start()
    {

    }

    protected override void Update()
    {
        base.Update();
    }

    //IGrounded Interface Methods
    public void CheckForGrounded()
    {

    }

    //IMoveable Interface Methods
    public void CheckForCanMove()
    {

    }

    //IActionable Interface Methods
    public void CheckForCanAct()
    {

    }
}

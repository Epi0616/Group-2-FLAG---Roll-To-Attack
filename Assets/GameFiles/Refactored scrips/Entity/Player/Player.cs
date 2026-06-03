using UnityEngine;
using System;

public class Player : Entity, IMoveable, IActionable, IGrounded, IUsesEntityInput
{
    //IUsesEntityInput Interface properties
    public EntityInputManager inputManager { get; set; }
    public bool canUseInput { get; set; }

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
        inputManager = GetComponent<EntityInputManager>();
        movementSpeed = new Stat(5f);
        movementController = new MovementController(this, new BaseMovementState(this));
    }

    protected override void Update()
    {
        base.Update();
        movementController.Update();
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

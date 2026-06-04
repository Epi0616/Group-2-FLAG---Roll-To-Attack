using UnityEngine;
using System;
using System.Collections.Generic;

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

    //PLAYER BASED PROPERTIES
    public List<ConditionalMovementDescriptor> movementDescriptors = new List<ConditionalMovementDescriptor>();
    private List<ConditionalMovement> movements = new List<ConditionalMovement>();


    private void Start()
    {
        inputManager = GetComponent<EntityInputManager>();
        movementSpeed = new Stat(5f);

        foreach (var movement in movementDescriptors)
        {
            movements.Add(movement.Create());
        }
        movementController = new MovementController(this, movements);

        movementController.Initialize();
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

using UnityEngine;
using System;
using System.Collections.Generic;

public class Player : Entity, IMoveable, IActionable, IGrounded, IUsesEntityInput, IUsesRigidBody
{
    //IUsesEntityInput Interface properties
    public EntityInputManager inputManager { get; set; }
    public bool canUseInput { get; set; }

    //IGrounded Interface properties
    public bool isGrounded { get; set; }
    public LayerMask environmentMask { get; set; }

    [Header("IMoveable")]
    [SerializeField] private bool CanMove;
    [SerializeField] private Stat MovementSpeed = new Stat(5f);
    public bool canMove { get => CanMove; set => CanMove = value; }
    public Stat movementSpeed { get => MovementSpeed; set => MovementSpeed = value; }
    public MovementController movementController { get; set; }

    //IActionable Interface properties
    public ActionController actionController { get; set; }
    public bool canAct { get; set; }

    //IUsesRigidBody Interface properties
    public Rigidbody rb { get; set; }

    //PLAYER BASED PROPERTIES
    public List<ConditionalMovementDescriptor> movementDescriptors = new List<ConditionalMovementDescriptor>();
    private List<ConditionalMovement> movements = new List<ConditionalMovement>();

    public List<ConditionalActionDescriptor> actionDescriptors = new List<ConditionalActionDescriptor>();
    private List<ConditionalAction> actions = new List<ConditionalAction>();

    protected override void Start()
    {
        base.Start();
        inputManager = GetComponent<EntityInputManager>();
        rb = GetComponent<Rigidbody>();

        foreach (var movement in movementDescriptors)
        {
            movements.Add(movement.Create());
        }
        movementController = new MovementController(this, movements);
        movementController.Initialize();

        foreach (var action in actionDescriptors)
        {
            actions.Add(action.Create());
        }
        actionController = new ActionController(this, actions);
        actionController.Initialize();
    }

    protected override void Update()
    {
        base.Update();
        movementController.Update();
        actionController.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        movementController.FixedUpdate();
        actionController.FixedUpdate();
    }

    //IGrounded Interface Methods
    public void CheckForGrounded()
    {
        //Ray ray = new Ray(transform.position, Vector3.down);
        //isGrounded = Physics.SphereCast(ray, 0.4f, 1.5f, groundLayer);
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

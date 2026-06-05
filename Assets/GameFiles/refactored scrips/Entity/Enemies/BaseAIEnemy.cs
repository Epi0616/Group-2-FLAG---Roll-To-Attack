using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

public class BaseAIEnemy : AIDrivenEntity , IMoveable, IGrounded, IStunable, IKnockbackable, IUsesRigidBody, IActionable
{
    // IGrounded Interface Properties
    public bool isGrounded { get; set; }

    // IMoveable Interface Properties
    public bool canMove { get; set; }
    public Stat movementSpeed { get; set; }
    public MovementController movementController { get; set; }
    [SerializeField] private float moveSpeedBaseValue = 10;

    // IActionable Interface Properties
    public ActionController actionController { get; set; }
    public bool canAct { get; set; }

    // IUsesRigidBody Interface Properties
    public Rigidbody rb { get; set; }

    // IKnockbackable Interface Properties
    public Stat knockbackWeightMod { get; set; }
    public Stat slammedDamageMod { get; set; }
    public bool isBeingDisplaced { get; set; }

    // IStunable Interface Properties
    public bool canBeStunned { get; set; }

    // ENEMY MOVEMENT AND ACTION PROPERTIES
    public List<ConditionalMovementDescriptor> movementDescriptors = new List<ConditionalMovementDescriptor>();
    private List<ConditionalMovement> movements = new List<ConditionalMovement>();

    public List<ConditionalActionDescriptor> actionDescriptors = new List<ConditionalActionDescriptor>();
    private List<ConditionalAction> actions = new List<ConditionalAction>();

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if (rb == null || agent == null)
        {
            Debug.LogError("BaseAIEnemy: Required Component not found from GetComponent");
        }

        movementSpeed = new Stat(moveSpeedBaseValue);

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

        agent.speed = movementSpeed.GetFinalValue();
    }

    protected override void Update()
    {
        base.Update();
        movementController.Update();
        actionController.Update();
        
    }

    // IGrounded Interface Methods
    public void CheckForGrounded()
    {

    }

    // IMoveable Interface Methods
    public void CheckForCanMove()
    {
    }

    // IActionable Interface Methods
    public void CheckForCanAct()
    {

    }

    // IKnockbackable Interface Methods
    public void CheckForDisplacement()
    {

    }
}

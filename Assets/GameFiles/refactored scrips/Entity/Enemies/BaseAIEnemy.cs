using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class BaseAIEnemy : AIDrivenEntity , IMoveable, IGrounded, IStunable, IKnockbackable, IActionable
{
    // IGrounded Interface Properties
    [Header("IGrounded Properties")]
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private bool IsGrounded;
    public bool isGrounded { get => IsGrounded; set => IsGrounded = value; }
    public LayerMask groundLayer { get => GroundLayer; set => GroundLayer = value; }

    // IMoveable Interface Properties
    [Header("IMoveable Properties")]
    [SerializeField] private bool CanMove = true;
    [SerializeField] private Stat MovementSpeed = new Stat(5f);
    [SerializeField] private List<ConditionalMovementDescriptor> ConditionalMovementDescriptors = new List<ConditionalMovementDescriptor>();
    private List<ConditionalMovement> ConditionalMovements = new List<ConditionalMovement>();
    public bool canMove { get => CanMove; set => CanMove = value; }
    public Stat movementSpeed { get => MovementSpeed; set => MovementSpeed = value; }
    public List<ConditionalMovementDescriptor> conditionalMovementDescriptors { get => ConditionalMovementDescriptors; set => ConditionalMovementDescriptors = value; }
    public List<ConditionalMovement> conditionalMovements { get => ConditionalMovements; set => ConditionalMovements = value; }
    public MovementController movementController { get; set; }


    // IActionable Interface Properties
    [Header("IActionable Properties")]
    [SerializeField] private List<ConditionalActionDescriptor> ConditionalActionDescriptors = new List<ConditionalActionDescriptor>();
    private List<ConditionalAction> ConditionalActions = new List<ConditionalAction>();
    private bool CanAct = true;
    public List<ConditionalActionDescriptor> conditionalActionDescriptors { get => ConditionalActionDescriptors; set => ConditionalActionDescriptors = value; }
    public List<ConditionalAction> conditionalActions { get => ConditionalActions; set => ConditionalActions = value; }
    public ActionController actionController { get; set; }
    public bool canAct { get => CanAct; set => CanAct = value; }

    // IKnockbackable Interface Properties
    [Header("IKnockbackable Properties")]
    [SerializeField] private Stat WeightModifier = new Stat(1);
    [SerializeField] private Stat SlammedDMGMod = new Stat(1);
    [SerializeField] private bool IsBeingDisplaced = false;
    public Stat knockbackWeightMod { get => WeightModifier; set => WeightModifier = value; }
    public Stat slammedDamageMod { get => SlammedDMGMod; set => SlammedDMGMod = value; }
    public bool isBeingDisplaced { get => IsBeingDisplaced; set => IsBeingDisplaced = value; }

    // IStunable Interface Properties
    [SerializeField] private bool CanBeStunned = true;
    public bool canBeStunned { get => CanBeStunned; set => CanBeStunned = value; }

    


    //// ENEMY MOVEMENT AND ACTION PROPERTIES
    //public List<ConditionalMovementDescriptor> movementDescriptors = new List<ConditionalMovementDescriptor>();
    //[SerializeField] private List<ConditionalMovement> movements = new List<ConditionalMovement>();

    //public List<ConditionalActionDescriptor> actionDescriptors = new List<ConditionalActionDescriptor>();
    //[SerializeField] private List<ConditionalAction> actions = new List<ConditionalAction>();

    

    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player"); //needs to be moved into interface/system for finding target
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        //environmentMask = LayerMask.GetMask("Ground", "Collider Props", "Pedestal");        
        //slamBaseRange = 5f;
        //slamPositionOffset = Vector3.zero;
        //defaultSlamColour = Color.white;
        //slamChargeUpTime = 1.5f;


        if (rb == null || agent == null)
        {
            Debug.LogError("BaseAIEnemy: Required Component not found from GetComponent");
        }

        UnpackConditionalMovements();
        movementController.Initialize();

        UnpackConditionalActions();
        actionController.Initialize();

        

        statList.Add(movementSpeed);
        statList.Add(slammedDamageMod);
        statList.Add(knockbackWeightMod);

        
        agent.speed = movementSpeed.GetFinalValue();
        //EnableAIAgent();
    }

    protected override void Update()
    {
        base.Update();
        movementController.Update();
        actionController.Update();
        CheckForCanMove();
        CheckForCanAct();
        CheckForDisplacement();
        CheckForGrounded();
    }

    // IGrounded Interface Methods
    public void CheckForGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = (Physics.Raycast(ray, out hit, 1.3f, groundLayer));
        //IsGrounded = (Physics.Raycast(ray, out hit, 1.3f, environmentMask));
    }

    // IMoveable Interface Methods
    public void CheckForCanMove()
    {
        canMove = !(actionController.CheckForMovementBlockersAction() || statusSystem.CheckForMovementBlockersStatus());
        //canMove = !statusSystem.CheckForMovementBlockersStatus();
    }
    public void UnpackConditionalMovements()
    {
        foreach (var movement in ConditionalMovementDescriptors)
        {
            conditionalMovements.Add(movement.Create());
        }
        movementController = new MovementController(this, conditionalMovements);
    }

    // IActionable Interface Methods
    public void CheckForCanAct()
    {
        canAct = !statusSystem.CheckForActionBlockersStatus();
        if (!canAct)
        {
            actionController.InterruptAllActive();
        }
    }
    public void UnpackConditionalActions()
    {
        foreach (var action in conditionalActionDescriptors)
        {
            conditionalActions.Add(action.Create());
        }
        actionController = new ActionController(this, conditionalActions);
    }

    // IKnockbackable Interface Methods
    public void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
        if (isBeingDisplaced)
        {
            DisableAIAgent();
        }
        else
        {
            EnableAIAgent();
        }
    }

    
}

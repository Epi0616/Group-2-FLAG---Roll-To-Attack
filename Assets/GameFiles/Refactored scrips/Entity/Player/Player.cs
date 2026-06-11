using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Player : Entity, IMoveable, IActionable, IGrounded, IUsesEntityInput, IUsesRigidBody, IModifiableActions, IJumpable, ISlamActionRequirements, IPoisonSpawner
{
    [Header("IUsesEntityInput")]
    public EntityInputManager inputManager { get; set; }
    public bool canUseInput { get; set; }

    [Header("IGrounded")]
    [SerializeField] private LayerMask GroundLayer;
    public bool isGrounded { get; set; }
    public LayerMask groundLayer { get => GroundLayer; set => GroundLayer = value; }

    [Header("IMoveable")]
    [SerializeField] private bool CanMove = true;
    [SerializeField] private Stat MovementSpeed = new Stat(5f);
    [SerializeField] private List<ConditionalMovementDescriptor> ConditionalMovementDescriptors = new List<ConditionalMovementDescriptor>();
    private List<ConditionalMovement> ConditionalMovements = new List<ConditionalMovement>();
    public bool canMove { get => CanMove; set => CanMove = value; }
    public Stat movementSpeed { get => MovementSpeed; set => MovementSpeed = value; }
    public List<ConditionalMovementDescriptor> conditionalMovementDescriptors { get => ConditionalMovementDescriptors; set => ConditionalMovementDescriptors = value; }
    public List<ConditionalMovement> conditionalMovements { get => ConditionalMovements; set => ConditionalMovements = value; }
    public MovementController movementController { get; set; }

    [Header("IActionable")]
    [SerializeField] private List<ConditionalActionDescriptor> ConditionalActionDescriptors = new List<ConditionalActionDescriptor>();
    private List<ConditionalAction> ConditionalActions = new List<ConditionalAction>();
    private bool CanAct = true;
    public List<ConditionalActionDescriptor> conditionalActionDescriptors { get => ConditionalActionDescriptors; set => ConditionalActionDescriptors = value; }
    public List<ConditionalAction> conditionalActions { get => ConditionalActions; set => ConditionalActions = value; }
    public ActionController actionController { get; set; }
    public bool canAct { get => CanAct; set => CanAct = value; }

    [Header("IModifiableActions")]
    [SerializeField] private List <ModifiableActionDescriptor> ModifiableActionDescriptors = new List<ModifiableActionDescriptor>();
    private List<ModifiableAction> ModifiableActions = new List<ModifiableAction>();
    public List<ModifiableActionDescriptor> modifiableActionDescriptors { get => ModifiableActionDescriptors; set => ModifiableActionDescriptors = value; }
    public List<ModifiableAction> modifiableActions { get => ModifiableActions; set => ModifiableActions = value; }
    public ActionSelectionSystem actionSelectionSystem { get; set; }

    [Header("IUsesRigidBody")]
    public Rigidbody rb { get; set; }

    [Header("IJumpable")]
    [SerializeField] private Stat JumpHeight = new Stat(3f);
    [SerializeField] private Stat JumpSpeed = new Stat(8f);
    [SerializeField] private Stat ImpactSpeed = new Stat(10f);
    private bool IsJumping = false;
    public Stat jumpHeight { get => JumpHeight; set => JumpHeight = value; }
    public Stat jumpSpeed { get => JumpSpeed; set => JumpSpeed = value; }
    public Stat impactSpeed { get => ImpactSpeed; set => ImpactSpeed = value; }
    public bool canJump { get; set; }
    public bool isJumping { get => IsJumping; set => IsJumping = value; }

    [Header("ISlamActionRequirements")]
    //[SerializeField] private LayerMask GroundLayer;
    [SerializeField] private GameObject SlamImpactField;
    //public LayerMask groundLayer { get => GroundLayer; set => GroundLayer = value; }
    public GameObject slamImpactField { get => SlamImpactField; set => SlamImpactField = value; }

    [Header("IPoisonSpawner")]
    [SerializeField] private GameObject PoisonFieldObj;
    [SerializeField] private float FieldLifetime = 0f;
    [SerializeField] private int FieldTickDamage = 0;
    public GameObject poisonFieldObj { get => PoisonFieldObj; set => PoisonFieldObj = value; }
    public float fieldLifetime { get => FieldLifetime; set => FieldLifetime = value; }
    public int fieldTickDamage { get => FieldTickDamage; set => FieldTickDamage = value; }


    protected override void Start()
    {
        base.Start();
        inputManager = GetComponent<EntityInputManager>();
        inputManager.Initialise(this);
        rb = GetComponent<Rigidbody>();

        UnpackConditionalMovements();
        movementController.Initialize();

        UnpackConditionalActions();
        actionController.Initialize();

        UnpackModifiableActions();
        actionSelectionSystem = new ActionSelectionSystem(this);

        statList.Add(movementSpeed);
    }

    protected override void Update()
    {
        base.Update();
        movementController.Update();
        actionController.Update();
        CheckForGrounded();
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
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.SphereCast(ray, 0.4f, 1, groundLayer);
    }

    //IMoveable Interface Methods
    public void CheckForCanMove()
    {
    }
    public void UnpackConditionalMovements()
    {
        foreach (var movement in ConditionalMovementDescriptors)
        {
            conditionalMovements.Add(movement.Create());
        }
        movementController = new MovementController(this, conditionalMovements);
    }

    //IActionable Interface Methods
    public void CheckForCanAct()
    {

    }
    public void UnpackConditionalActions()
    {
        foreach (var action in conditionalActionDescriptors)
        {
            conditionalActions.Add(action.Create());
        }
        actionController = new ActionController(this, conditionalActions);
    }

    //IJumpable Interface Methods
    public void CheckForCanJump()
    { 
    
    }

    //IModifiableActions Methods
    public void UnpackModifiableActions()
    { 
        modifiableActions.Clear();
        foreach (ModifiableActionDescriptor modifiableActionDescriptor in ModifiableActionDescriptors)
        {
            Debug.Log("added modif action ");
            modifiableActions.Add(modifiableActionDescriptor.Create());
        }
    }
}

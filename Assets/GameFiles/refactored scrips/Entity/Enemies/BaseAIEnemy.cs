using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

public class BaseAIEnemy : AIDrivenEntity , IMoveable, IGrounded, IStunable, IKnockbackable, IActionable, ISlamActionRequirements
{
    // IGrounded Interface Properties
    [Header("IGrounded Properties")]
    [SerializeField] private LayerMask EnvironmentMask;
    [SerializeField] private bool IsGrounded;
    public bool isGrounded { get => IsGrounded; set => IsGrounded = value; }
    public LayerMask environmentMask { get => EnvironmentMask; set => EnvironmentMask = value; }

    // IMoveable Interface Properties
    [Header("IMoveable Properties")]
    [SerializeField] private bool CanMove = true;
    [SerializeField] private Stat MovementSpeed = new Stat(10);
    public bool canMove { get => CanMove; set => CanMove = value; }
    public Stat movementSpeed { get => MovementSpeed; set => MovementSpeed = value; }
    public MovementController movementController { get; set; }


    // IActionable Interface Properties
    [Header("IActionable Properties")]
    [SerializeField] private bool CanAct = true;
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
    public bool canBeStunned { get; set; }

    // ISlamAction Interface Propertires
    [Header("ISlam Required Properties")]
    [SerializeField] float SlamRange = 5;
    [SerializeField] Vector3 SlamOriginOffset = Vector3.zero;
    [SerializeField] Color SlamColour = Color.white;
    [SerializeField] GameObject prefab;
    public float slamBaseRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamOriginOffset; set => SlamOriginOffset = value; }
    public Color defaultSlamColour { get => SlamColour; set => SlamColour = value; }
    public float slamChargeUpTime { get; set; }
    public GameObject DebugSlamObj { get => prefab; set => prefab = value; }

    // ENEMY MOVEMENT AND ACTION PROPERTIES
    public List<ConditionalMovementDescriptor> movementDescriptors = new List<ConditionalMovementDescriptor>();
    private List<ConditionalMovement> movements = new List<ConditionalMovement>();

    public List<ConditionalActionDescriptor> actionDescriptors = new List<ConditionalActionDescriptor>();
    private List<ConditionalAction> actions = new List<ConditionalAction>();

    

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        environmentMask = LayerMask.GetMask("Ground", "Collider Props", "Pedestal");        
        //slamBaseRange = 5f;
        //slamPositionOffset = Vector3.zero;
        //defaultSlamColour = Color.white;
        //slamChargeUpTime = 1.5f;


        if (rb == null || agent == null)
        {
            Debug.LogError("BaseAIEnemy: Required Component not found from GetComponent");
        }

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

        

        statList.Add(movementSpeed);
        statList.Add(slammedDamageMod);
        statList.Add(knockbackWeightMod);

        //movements.Add(new ConditionalMovement(new NavMeshMovement(), new List<ICondition>() { new AlwaysTrueCondition() }));
        agent.speed = movementSpeed.GetFinalValue();
        EnableAIAgent();
    }

    protected override void Update()
    {
        base.Update();
        movementController.Update();
        actionController.Update();
        CheckForCanMove();
    }

    // IGrounded Interface Methods
    public void CheckForGrounded()
    {

    }

    // IMoveable Interface Methods
    public void CheckForCanMove()
    {
        canMove = !actionController.CheckForMovementBlockersAction();
    }

    // IActionable Interface Methods
    public void CheckForCanAct()
    {
        
    }

    // IKnockbackable Interface Methods
    public void CheckForDisplacement()
    {

    }

    public GameObject SPAWNTHING(GameObject thing, Vector3 pos)
    {
        return Instantiate(thing, pos, Quaternion.identity);
    }
}

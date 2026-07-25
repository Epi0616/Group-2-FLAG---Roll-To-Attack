using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : Entity, IMoveable, IActionable, IGrounded, IUsesEntityInput, IUsesRigidBody, IModifiableActions, IJumpable, ISlamActionRequirements, IPoisonSpawner, IRocketSpawner,
    IOrbitSpikeSpawner, IVacuumSpawner, IKnockbackFieldSpawner, ISlowBubbleSpawner, ITarget, IIconDisplayer
{
    [Header("IUsesEntityInput")]
    public EntityInputManager inputManager { get; set; }
    public bool canUseInput { get; set; }

    [Header("IGrounded")]
    [SerializeField] private GameObject GroundCheckCastPoint;
    [SerializeField] private LayerMask GroundLayer;
    public GameObject groundCheckCastPoint { get => GroundCheckCastPoint; set => GroundCheckCastPoint = value; }
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
    [SerializeField] private List<ConditionalAction> ConditionalActions = new List<ConditionalAction>();
    [SerializeField] private bool CanAct = true;
    public List<ConditionalAction> conditionalActions { get => ConditionalActions; set => ConditionalActions = value; }
    public ActionController actionController { get; set; }
    public bool canAct { get => CanAct; set => CanAct = value; }

    [Header("IModifiableActions")]
    [SerializeField] private List<ModifiableActionDescriptor> ModifiableActionDescriptors = new List<ModifiableActionDescriptor>();
    [SerializeField] private PlayerLoadOut PlayerLoadOut;
    private List<ModifiableAction> ModifiableActions = new List<ModifiableAction>();
    private List<ModifiableAction> ModifiableActionStorage = new List<ModifiableAction>();
    public List<ModifiableAction> modifiableActions { get => ModifiableActions; set => ModifiableActions = value; }
    public List<ModifiableAction> modifiableActionStorage { get => ModifiableActionStorage; set => ModifiableActionStorage = value; }
    public ActionSelectionSystem actionSelectionSystem { get; set; }
    public PlayerLoadOut playerLoadOut { get => PlayerLoadOut; set => PlayerLoadOut = value; }

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
    [SerializeField] private LayerMask PedestalLayer;
    //public LayerMask groundLayer { get => GroundLayer; set => GroundLayer = value; }
    public LayerMask pedestalLayer { get => PedestalLayer; set => PedestalLayer = value; }
    public GameObject slamImpactField { get => SlamImpactField; set => SlamImpactField = value; }

    [Header("ITargetRequirements")]
    [SerializeField] private int PerimeterPointsCount = 0;
    [SerializeField] private float PerimeterRadius = 0;
    public int perimeterPointsCount { get => PerimeterPointsCount; set => PerimeterPointsCount = value; }
    public float perimeterRadius { get => PerimeterRadius; set => PerimeterRadius = value; }
    public List<Vector3> perimeterPoints { get; set; }

    [Header("IPoisonSpawner")]
    [SerializeField] private GameObject PoisonFieldObj;
    [SerializeField] private GameObject EnhancedPoisonFieldObj;
    [SerializeField] private float FieldLifetime = 0f;
    [SerializeField] private int FieldTickDamage = 0;
    public GameObject poisonFieldObj { get => PoisonFieldObj; set => PoisonFieldObj = value; }
    public GameObject enhancedPoisonFieldObj { get => EnhancedPoisonFieldObj; set => EnhancedPoisonFieldObj = value; }
    public float fieldLifetime { get => FieldLifetime; set => FieldLifetime = value; }
    public int fieldTickDamage { get => FieldTickDamage; set => FieldTickDamage = value; }

    [Header("IRocketSpawner")]
    [SerializeField] private GameObject RocketObj;
    [SerializeField] private GameObject EnhancedRocketObj;
    [SerializeField] private int RocketDamage = 0;
    public GameObject rocketObj { get => RocketObj; set => RocketObj = value; }
    public GameObject enhancedRocketObj { get => EnhancedRocketObj; set => EnhancedRocketObj = value; }
    public int rocketDamage { get => RocketDamage; set => RocketDamage = value; }

    [Header("IOrbitSpikeSpawner")]
    [SerializeField] private GameObject SpikePrefab;
    [SerializeField] private GameObject EnhancedSpikePrefab;
    [SerializeField] private float SpikeLifeSpan = 0f;
    [SerializeField] private float OrbitRadius = 0f;
    [SerializeField] private float InitialOrbitSpeed = 0f;
    [SerializeField] private int SpikeDamaged = 0;
    private List<BaseOrbitObject> OrbitObjects = new List<BaseOrbitObject>();

    public GameObject spikePrefab { get => SpikePrefab; set => SpikePrefab = value; }
    public GameObject enhancedSpikePrefab { get => EnhancedSpikePrefab; set => EnhancedSpikePrefab = value; }
    public float spikeLifeSpan { get => SpikeLifeSpan; set => SpikeLifeSpan = value; }
    public float orbitRadius { get => OrbitRadius; set => OrbitRadius = value; }
    public float initialOrbitSpeed { get => InitialOrbitSpeed; set => InitialOrbitSpeed = value; }
    public int spikeDamage { get => SpikeDamaged; set => SpikeDamaged = value; }
    public List<BaseOrbitObject> orbitObjects { get => OrbitObjects; set => OrbitObjects = value; }


    [Header("IVacuumSpawner")]
    [SerializeField] private GameObject MineObj;
    [SerializeField] private GameObject EnhancedMineObj;
    [SerializeField] private float MineChargeTime = 0f;
    public GameObject mineObj { get => MineObj; set => MineObj = value; }
    public GameObject enhancedMineObj { get => EnhancedMineObj; set => EnhancedMineObj = value; }
    public float mineChargeTime { get => MineChargeTime; set => MineChargeTime = value; }

    [Header("IKnockbackFieldSpawner")]
    [SerializeField] private GameObject KBFieldPrefab;
    public GameObject knockbackFieldPrefab { get => KBFieldPrefab; set => KBFieldPrefab = value; }

    [Header("ISlowBubbleSpawner")]
    [SerializeField] private GameObject SlowingBubblePrefab;
    public GameObject slowBubblePrefab { get => SlowingBubblePrefab; set => SlowingBubblePrefab = value; }
    public EnhancedSlowingBubble currentBubbleInstance { get; set; }

    [Header("IIconDisplayer")]
    [SerializeField] private GameObject DisplayPlanePrefab;
    public GameObject displayPlanePrefab { get => DisplayPlanePrefab; set => DisplayPlanePrefab = value; }
    protected override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        base.Initialize();

        SetGroundedCheckPoint();
        rb = GetComponent<Rigidbody>();

        inputManager = GetComponent<EntityInputManager>();
        inputManager.Initialise(this);

        InitializePerimeterPoints();

        UnpackConditionalMovements();
        movementController.Initialize();

        UnpackConditionalActions();
        actionController.Initialize();

        actionSelectionSystem = new ActionSelectionSystem(this);
        UnpackModifiableActions();

        statList.Add(movementSpeed);
    }

    protected override void Update()
    {
        base.Update();
        movementController.Update();
        actionController.Update();
        CheckForGrounded();

        RunTimeStatTracker.totalTimeSurvived += Time.deltaTime;
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
        isGrounded = Physics.SphereCast(ray, 0.9f, 1, groundLayer);
    }

    public void SetGroundedCheckPoint()
    {
        if (!groundCheckCastPoint)
        {
            groundCheckCastPoint = gameObject;
        }
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
        foreach (var action in ConditionalActionDescriptors)
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
        List<ModifiableAction> modifiableActions = new List<ModifiableAction>();
        foreach (ModifiableActionDescriptor modifiableActionDescriptor in ModifiableActionDescriptors)
        {
            modifiableActions.Add(modifiableActionDescriptor.Create());
        }

        actionSelectionSystem.SetModifiableActions(modifiableActions);
    }

    //IOrbitSpikeSpawner Methods
    public void RemoveObjectFromOrbit(BaseOrbitObject obj)
    {
        orbitObjects.Remove(obj);
        //UpdateOrbitObjectAngles();
    }

    public void UpdateOrbitObjectAngles()
    {
        for (int i = 0; i < orbitObjects.Count; i++)
        {
            float angle = i * (360f / orbitObjects.Count);
            orbitObjects[i].UpdateAngle(angle);
        }
        //Debug.Log("There are currently: " + orbitObjects.Count + " objects in orbit");
    }

    public void RefreshSpikeAge()
    {
        for (int i = 0; i < orbitObjects.Count; i++)
        {
            orbitObjects[i].age = 0;
        }
    }

    public void EjectEnhancedSpikes()
    {
        for (int i = orbitObjects.Count - 1; i >= 0; i--)
        {
            if (orbitObjects[i] is EnhancedOrbitingSpike EOS)
            {
                EOS.DropOff();
            }
        }
    }

    //ITarget methods
    public void InitializePerimeterPoints()
    {
        perimeterPoints = new List<Vector3>();
    }

    public void GeneratePerimeterPoints()
    {
        List<Vector3> chosenPoints = new List<Vector3>();

        float angleStep = 360 / perimeterPointsCount;

        for (int i = 0; i < perimeterPointsCount; i++)
        {
            float angle = angleStep * i;
            float angleInRad = angle * Mathf.Deg2Rad;
            Vector3 pointToCheck = transform.position;
            pointToCheck.x += perimeterRadius * Mathf.Cos(angleInRad);
            pointToCheck.y = 1.7f; //hard coded to be the height of the arena for now, will adjust at some point...
            pointToCheck.z += perimeterRadius * Mathf.Sin(angleInRad);

            NavMeshHit hit;
            NavMesh.SamplePosition(pointToCheck, out hit, 2, NavMesh.AllAreas);

            if (hit.hit)
            { 
                chosenPoints.Add(hit.position);
            }
        }

        perimeterPoints = chosenPoints;
    }
}

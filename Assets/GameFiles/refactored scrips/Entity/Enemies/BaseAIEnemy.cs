using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class BaseAIEnemy : AIDrivenEntity , IMoveable, IGrounded, IStunable, IKnockbackable, IActionable, ISlamActionRequirements, IPoisonSpawner, IOrbitSpikeSpawner, IVacuumSpawner, IRocketSpawner
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
    [SerializeField] private bool CanBeStunned = true;
    public bool canBeStunned { get => CanBeStunned; set => CanBeStunned = value; }

    // ISlamAction Interface Propertires
    [Header("ISlam Required Properties")]
    [SerializeField] GameObject ImpactFieldPrefab;
    public GameObject SlamImpactField { get => ImpactFieldPrefab; set => ImpactFieldPrefab = value; }

    // IPoisonSpawner Interface
    [Header("IPoison Required Properties")]
    [SerializeField] private GameObject PoisonFieldPrefab;
    [SerializeField] private float PoisonFieldLifeTime = 5;
    [SerializeField] private int PoisonFieldDamageTick;
    public GameObject PoisonFieldObj { get => PoisonFieldPrefab; set => PoisonFieldPrefab = value; }
    public float fieldLifetime { get => PoisonFieldLifeTime; set => PoisonFieldLifeTime = value; }
    public int fieldTickDamage { get => PoisonFieldDamageTick; set => PoisonFieldDamageTick = value; }

    // IOrbitSpikeSpawner Interface
    private List<BaseOrbitObject> orbitObj = new List<BaseOrbitObject>();
    public List<BaseOrbitObject> orbitObjects { get => orbitObj; set => orbitObj = value; }

    [Header("IOrbitSpike Required Properties")]
    //[SerializeField] private int NumberOfSpikesPerSpawn = 5;
    //public int numSpikesPerSpawn { get => NumberOfSpikesPerSpawn; set => NumberOfSpikesPerSpawn = value; }
    [SerializeField] private float SpikeLifeSpan = 10;
    public float spikeLifeSpan { get => SpikeLifeSpan; set => SpikeLifeSpan = value; }
    [SerializeField] private float SpikeOrbitRadius = 4;
    public float orbitRadius { get => SpikeOrbitRadius; set => SpikeOrbitRadius = value; }
    [SerializeField] private float SpikeOrbitSpeed = 15;
    public float initialOrbitSpeed { get => SpikeOrbitSpeed; set => SpikeOrbitSpeed = value; }
    [SerializeField] private int SpikeDamage;
    public int spikeDamage { get => SpikeDamage; set => SpikeDamage = value; }
    [SerializeField] private GameObject SpikePrefab;
    public GameObject spikePrefab { get => SpikePrefab; set => SpikePrefab = value; }

    // IVacuumSpawner Interface

    [SerializeField] private float VacuumMineDetonationTime = 5;
    public float mineChargeTime { get => VacuumMineDetonationTime; set => VacuumMineDetonationTime = value; }
    [SerializeField] private GameObject VacuumMinePrefab;
    public GameObject minePrefab { get => VacuumMinePrefab; set => VacuumMinePrefab = value; }

    // IRocketSpawner intercae
    [SerializeField] private int RocketDamage;
    public int rocketDamage { get => RocketDamage; set => RocketDamage = value; }
    [SerializeField] private GameObject RocketPrefab;
    public GameObject rocketPrefab { get => RocketPrefab; set => RocketPrefab = value; }


    // ENEMY MOVEMENT AND ACTION PROPERTIES
    public List<ConditionalMovementDescriptor> movementDescriptors = new List<ConditionalMovementDescriptor>();
    [SerializeField] private List<ConditionalMovement> movements = new List<ConditionalMovement>();

    public List<ConditionalActionDescriptor> actionDescriptors = new List<ConditionalActionDescriptor>();
    [SerializeField] private List<ConditionalAction> actions = new List<ConditionalAction>();

    

    protected override void Start()
    {
        base.Start();
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

        foreach (var movement in movementDescriptors)
        {
            movements.Add(movement.Create());
        }
        //movements.Add(new ConditionalMovement(new NavMeshMovement(), new List<ICondition>() { new CanMoveCondition() }));
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

    // IActionable Interface Methods
    public void CheckForCanAct()
    {
        canAct = !statusSystem.CheckForActionBlockersStatus();
        if (!canAct)
        {
            actionController.InterruptAllActive();
        }
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

    // IOrbitSpike Interface Methods

    public void RemoveObjectFromOrbit(BaseOrbitObject obj)
    {
        orbitObjects.Remove(obj);
    }   

    public void UpdateOrbitObjectAngles()
    {
        for (int i = 0; i < orbitObjects.Count; i++)
        {
            float angle = i * (360f / orbitObjects.Count);
            orbitObjects[i].UpdateAngle(angle);            
        }
    }
}

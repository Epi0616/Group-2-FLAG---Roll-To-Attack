using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Localization;

public class BaseAIEnemy : AIDrivenEntity,
    IMoveable, 
    IGrounded, 
    IStunable, 
    IKnockbackable, 
    IActionable, 
    IAnimated, 
    ISpawnModifier, 
    IResetable, 
    IWaveEnemy
{
    [Header("IGrounded Properties")]
    [Tooltip("Dont Assign unless necessary")]
    [SerializeField] private GameObject GroundCheckCastPoint;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] private bool IsGrounded;
    public GameObject groundCheckCastPoint { get => GroundCheckCastPoint; set => GroundCheckCastPoint = value; }
    public bool isGrounded { get => IsGrounded; set => IsGrounded = value; }
    public LayerMask groundLayer { get => GroundLayer; set => GroundLayer = value; }
    public float groundCheckDistance { get => GroundCheckDistance; set => GroundCheckDistance = value; }

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

    [Header("IActionable Properties")]
    [SerializeField] private List<ConditionalActionDescriptor> ConditionalActionDescriptors = new List<ConditionalActionDescriptor>();
    [SerializeField] private List<ConditionalAction> ConditionalActions = new List<ConditionalAction>();
    [SerializeField] private bool CanAct = true;
    public List<ConditionalActionDescriptor> conditionalActionDescriptors { get => ConditionalActionDescriptors; set => ConditionalActionDescriptors = value; }
    public List<ConditionalAction> conditionalActions { get => ConditionalActions; set => ConditionalActions = value; }
    public ActionController actionController { get; set; }
    public bool canAct { get => CanAct; set => CanAct = value; }

    [Header("IKnockbackable Properties")]
    [SerializeField] private Stat WeightModifier = new Stat(1);
    [SerializeField] private Stat SlammedDMGMod = new Stat(1);
    [SerializeField] private bool IsBeingDisplaced = false;
    public Stat knockbackWeightMod { get => WeightModifier; set => WeightModifier = value; }
    public Stat slammedDamageMod { get => SlammedDMGMod; set => SlammedDMGMod = value; }
    public bool isBeingDisplaced { get => IsBeingDisplaced; set => IsBeingDisplaced = value; }

    [Header("IStunable Properties")]
    [SerializeField] private bool CanBeStunned = true;
    [SerializeField] private bool IsStunned;
    [SerializeField] private float StunInterval = 0;
    [SerializeField] private float CurrentStunInterval = 0;
    public bool canBeStunned { get => CanBeStunned; set => CanBeStunned = value; }
    public bool isStunned { get => IsStunned; set => IsStunned = value; }
    public float stunInterval { get => StunInterval; set => StunInterval = value; }
    public float currentStunInterval { get => CurrentStunInterval; set => CurrentStunInterval = value; }

    [Header("IAnimated Properties")]
    [SerializeField] private AnimationOnDemandManager AnimationManager;
    public AnimationOnDemandManager animationManager { get => AnimationManager; set => AnimationManager = value; }

    [Header("ISpawnModifier Properties")]
    [SerializeField] private SpawnModifier SpawnModifier;
    public SpawnModifier spawnModifier { get => SpawnModifier; set => SpawnModifier = value; }

    [Header("IWaveEnemy Properties")]
    [SerializeField] private bool IsWaveEnemy = false;
    public bool isWaveEnemy { get => IsWaveEnemy; set => IsWaveEnemy = value; }

    //// ENEMY MOVEMENT AND ACTION PROPERTIES
    //public List<ConditionalMovementDescriptor> movementDescriptors = new List<ConditionalMovementDescriptor>();
    //[SerializeField] private List<ConditionalMovement> movements = new List<ConditionalMovement>();

    //public List<ConditionalActionDescriptor> actionDescriptors = new List<ConditionalActionDescriptor>();
    //[SerializeField] private List<ConditionalAction> actions = new List<ConditionalAction>();

    public LocalizedString SlammedString;
    public LocalizedString ShatteredString;
    public LocalizedString CrushedString;

    protected override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        base.Initialize();
        target = GameObject.FindGameObjectWithTag("Player"); //needs to be moved into interface/system for finding target

        //environmentMask = LayerMask.GetMask("Ground", "Collider Props", "Pedestal");        
        //slamBaseRange = 5f;
        //slamPositionOffset = Vector3.zero;
        //defaultSlamColour = Color.white;
        //slamChargeUpTime = 1.5f;

        SetGroundedCheckPoint();
        if (rb == null || agent == null)
        {
            Debug.LogError("BaseAIEnemy: Required Component not found from GetComponent");
        }

        animationManager.Initialize();

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

        CheckForCanMove();
        CheckForCanAct();
        CheckForDisplacement();
        CheckForGrounded();
        CheckForStunned();

        movementController.Update();
        actionController.Update();
    }

    //IResetable
    public override void Reset()
    {
        base.Reset();
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player"); //same as in initialize, move to a interface/function responsible for finding a target
        if (movementController != null)
            movementController.Reset();
        if (actionController != null)
            actionController.Reset();

        //UnpackConditionalActions();
        //UnpackConditionalMovements();
    }

    // IGrounded Interface Methods
    public virtual void CheckForGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(groundCheckCastPoint.transform.position, Vector3.down);
        isGrounded = (Physics.Raycast(ray, out hit, groundCheckDistance, groundLayer));
        //IsGrounded = (Physics.Raycast(ray, out hit, 1.3f, environmentMask));
    }
    public void SetGroundedCheckPoint()
    {
        if (groundCheckCastPoint == null)
        {
            groundCheckCastPoint = gameObject;
        }
    }

    // IMoveable Interface Methods
    public void CheckForCanMove()
    {
        canMove = !(actionController.CheckForMovementBlockersAction() || statusSystem.CheckForMovementBlockersStatus());
        //canMove = !statusSystem.CheckForMovementBlockersStatus();
        //Debug.Log(statusSystem.CheckForMovementBlockersStatus());
    }
    public void UnpackConditionalMovements()
    {
        conditionalMovements.Clear();
        foreach (var movement in ConditionalMovementDescriptors)
        {
            conditionalMovements.Add(movement.Create());
        }
        movementController = new MovementController(this, conditionalMovements);
    }

    // IActionable Interface Methods
    public virtual void CheckForCanAct()
    {
        canAct = !statusSystem.CheckForActionBlockersStatus();
        if (!canAct)
        {
            actionController.InterruptInterruptableActions(); //was originally interrupt all, im guessing this is undesireable as some actions like split on death for the slime NEED to happen?
        }
    }
    public void UnpackConditionalActions()
    {
        conditionalActions.Clear();
        foreach (var action in conditionalActionDescriptors)
        {
            conditionalActions.Add(action.Create());
        }
        actionController = new ActionController(this, conditionalActions);
    }

    // IKnockbackable Interface Methods
    public virtual void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
        if (isBeingDisplaced)
        {
            DisableAIAgent();
        }
        else if (canMove)
        {
            EnableAIAgent();
        }
    }

    //IStunable Interface Methods
    public void CheckForStunned()
    {
        isStunned = statusSystem.CheckForStunnedStatus();
        currentStunInterval -= Time.deltaTime;

        if (currentStunInterval > 0)
        {
            canBeStunned = false;
            return;
        }

        canBeStunned = true;
    }

    public void ResetStunInterval()
    {
        currentStunInterval = stunInterval;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Environment") && !collision.gameObject.CompareTag("Pedestal")) { return; }
        if (!isBeingDisplaced) { return; }
        if (!(rb.linearVelocity.magnitude > 10)) return;

        float dmgMod = Mathf.Clamp(slammedDamageMod.GetFinalValue(), 1.0f, 5.0f);
        int appliedDamage = (int)((collision.impulse.magnitude / 3) * dmgMod);

        if (appliedDamage < 25) { appliedDamage = 25; }


        if (statusSystem.CheckForStatusByType(StatusType.Freeze))
        {
            //Debug.Log("Shattered " + statusSystem.CheckForStatusByType(StatusType.Freeze));
            //AudioManager.instance.PlayRandomSoundClip(EnemyShatteredSounds);
            textDisplaySystem.DisplayHigherText("SHATTERED", Color.deepSkyBlue, 52);
            //textDisplaySystem.DisplayHigherText(ShatteredString.GetLocalizedString(), Color.deepSkyBlue, 52);
            OnTakeDamage(appliedDamage, Color.deepSkyBlue, DamageType.Shattered);
            
        }
        else if (statusSystem.CheckForStatusByType(StatusType.Crumbling))
        {
            //Debug.Log("Crushed " + statusSystem.CheckForStatusByType(StatusType.Crumbling));
            //AudioManager.instance.PlayRandomSoundClip(EnemyWallSlamSounds);
            textDisplaySystem.DisplayHigherText("CRUSHED", Color.sienna, 52);
            //textDisplaySystem.DisplayHigherText(CrushedString.GetLocalizedString(), Color.sienna, 52);
            OnTakeDamage(appliedDamage, Color.sienna, DamageType.Slammed);
        }
        else
        {
            //Debug.Log("Slammed");
            textDisplaySystem.DisplayHigherText("SLAMMED", Color.darkGoldenRod, 52);
           // textDisplaySystem.DisplayHigherText(SlammedString.GetLocalizedString(), Color.darkGoldenRod, 52);
            OnTakeDamage(appliedDamage, Color.darkGoldenRod, DamageType.Slammed);
        }
        //statusSystem.RemoveEffectByType(StatusType.Knockback);

        // Eventual VFX/SFX can go here for wall slams
        // add a check for the value of dmgMod to increase volume/size of effects
    }
}

using UnityEngine;

public class SlimeBoss : BaseBossEnemy, 
    ISlimeTrail, 
    IGrounded, 
    IKnockbackable, 
    ISlimeSplit
{
    [Header("ISlimeTrail")]
    [SerializeField] private GameObject SlimeFieldObj;
    [SerializeField] private LayerMask SlimeableMask;
    [SerializeField] private bool IsCharging;
    public GameObject slimeFieldObj { get => SlimeFieldObj; set => SlimeFieldObj = value; }
    public LayerMask slimeableMask { get => SlimeableMask; set => SlimeableMask = value; }
    public bool isCharging { get => IsCharging; set => IsCharging = value; }

    [Header("ISlimeSplit")]
    [SerializeField] private GameObject ChildObj;
    [SerializeField] private int ChildrenSpawned;
    [SerializeField] private int IterationsLeft;
    [SerializeField] private float Scale;

    public GameObject childObj { get => ChildObj; set => ChildObj = value; }
    public int childrenSpawned { get => ChildrenSpawned; set => ChildrenSpawned = value; }
    public int iterationsLeft { get => IterationsLeft; set => IterationsLeft = value; }
    public float scale { get => Scale; set => Scale = value; }

    public override void EnableAIAgent()
    {
        if (!isAIDisabled) { return; }

        rb.isKinematic = false;
        //rb.linearDamping = 0f;

        //rb.useGravity = false;
        //rb.isKinematic = true;

        isAIDisabled = false;

        //agent.enabled = true;      
        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.Warp(transform.position);
        agent.ResetPath();
    }

    public override void DisableAIAgent()
    {
        if (isAIDisabled) { return; }

        //agent.enabled = false;
        agent.updatePosition = false;
        agent.updateRotation = false;

        isAIDisabled = true;

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearDamping = 2f;
    }

    public override void CheckForGrounded()
    {
        Ray ray = new Ray(groundCheckCastPoint.transform.position, Vector3.down);
        isGrounded = Physics.SphereCast(ray, 5f * scale, groundCheckDistance, groundLayer);
    }

    public override void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
        if (isBeingDisplaced)
        {
            DisableAIAgent();
        }
        else if (canMove && rb.linearVelocity.magnitude < 1)
        { 
            EnableAIAgent();
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        
    }
}

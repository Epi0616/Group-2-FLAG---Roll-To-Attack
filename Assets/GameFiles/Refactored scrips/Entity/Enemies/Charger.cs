using UnityEngine;

public class Charger : BaseAIEnemy,
    ICrashCollider,
    IRadialProjectile
{
    [Header("ICrashCollider")]
    [SerializeField] Collider CrashCollider;
    private Vector3 CrashPosition = Vector3.zero;
    public Collider crashCollider { get => CrashCollider; private set => CrashCollider = value; }
    public Vector3 crashPosition { get => CrashPosition; private set => CrashPosition = value; }
    public bool hasCrashed { get; set; }

    [Header("IFireWingBeat")]
    [SerializeField] GameObject DebrisObj;
    public GameObject radialObj { get => DebrisObj; set => DebrisObj = value; }

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
        //rb.linearDamping = 0.5f;
    }

    public override void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
        if (isBeingDisplaced)
        {
            DisableAIAgent();
        }
        else if (canMove && rb.linearVelocity.magnitude < 2)
        {
            EnableAIAgent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCrashed == false)
        {
            crashPosition = other.ClosestPoint(transform.position);
        }
        hasCrashed = true;
    }
}

using UnityEngine;

public class VacuumDisplacementEffect : BaseDisplacementEffect
{
    Vector3 origin;
    float force;

    public VacuumDisplacementEffect(Vector3 origin, float force)
    {
        this.origin = origin;
        this.force = force;
        type = StatusType.Knockback;
        preventsMovement = true;
        preventsAction = true;
        isDisplacing = true;
        isStackable = true;
    }

    protected override void OnApplication()
    {
        base.OnApplication();
        if (!isActive) return;
        //Debug.Log("Active Vacuum Applied");
        rbInterfaceAccess.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (entityRef.transform.position - origin);

        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0.2f;
        rbInterfaceAccess.rb.AddForce(targetDirection * ((force * knockbackInterfaceAccess.knockbackWeightMod.GetFinalValue()) * 10f), ForceMode.VelocityChange);
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
    }
}
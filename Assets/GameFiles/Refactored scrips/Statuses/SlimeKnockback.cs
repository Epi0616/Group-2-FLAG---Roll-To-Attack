using UnityEngine;

public class SlimeKnockback : SlimeDisplacement
{
    Vector3 origin;
    float force;

    public SlimeKnockback(Vector3 origin, float force)
    {
        this.origin = origin;
        this.force = force;
        type = StatusType.Knockback;
        preventsMovement = true;
        isDisplacing = true;
        isStackable = true;
    }

    protected override void OnApplication()
    {
        base.OnApplication();

        if (!isActive) { return; }

        rbInterfaceAccess.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (entityRef.transform.position - origin);

        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0.3f;
        rbInterfaceAccess.rb.AddForce(targetDirection * ((force * knockbackInterfaceAccess.knockbackWeightMod.GetFinalValue()) * 10f), ForceMode.VelocityChange);
    }

    protected override void OnRemoval()
    {

        base.OnRemoval();
        Debug.Log("Slime KB Removed");
    }
}

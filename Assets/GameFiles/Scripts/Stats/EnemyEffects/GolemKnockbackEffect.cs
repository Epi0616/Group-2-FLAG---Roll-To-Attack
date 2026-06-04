using UnityEngine;

public class GolemKnockBackEffect : BaseDisplacementEffect
{
    Vector3 origin;
    float force;

    public GolemKnockBackEffect(Vector3 origin, float force)
    {
        this.origin = origin;
        this.force = force;
        type = StatusType.Knockback;
    }

    protected override void OnApplication()
    {
        base.OnApplication();     

        //entityRef.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (entityRef.transform.position - origin);

        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 1.2f;
        //entityRef.rb.AddForce(targetDirection * ((force * knockbackInterfaceAccess.knockbackWeightMod.GetFinalValue()) * 10f), ForceMode.VelocityChange);
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
    }
}

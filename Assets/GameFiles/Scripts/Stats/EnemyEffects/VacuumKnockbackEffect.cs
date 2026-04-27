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
    }

    protected override void OnApplication()
    {
        base.OnApplication();

        enemyRef.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (enemyRef.transform.position - origin);

        Vector3 targetDirection = targetVector.normalized;

        targetDirection.y = 0.1f;
        targetDirection.x *= 1.5f;
        targetDirection.z *= 1.5f;

        enemyRef.rb.AddForce(targetDirection * ((force * enemyRef.knockbackWeightModifierStat.GetFinalValue()) * 10f), ForceMode.VelocityChange);
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
    }
}
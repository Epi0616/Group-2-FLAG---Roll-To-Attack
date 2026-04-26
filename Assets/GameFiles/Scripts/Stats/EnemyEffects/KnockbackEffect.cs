using UnityEngine;

public class KnockbackEffect : BaseDisplacementEffect
{
    Vector3 origin;
    float force;
    
    public KnockbackEffect(Vector3 origin, float force)
    {
        this.origin = origin;
        this.force = force;
    }

    protected override void OnApplication()
    {
        base.OnApplication();

        enemyRef.DisableAI();

        enemyRef.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (enemyRef.transform.position - origin);

        Vector3 targetDirection = targetVector.normalized;

        targetDirection.y = 0.3f;

        enemyRef.rb.AddForce(targetDirection * ((force * enemyRef.knockbackWeightModifierStat.GetFinalValue()) * 10f), ForceMode.VelocityChange);
    } 

    protected override void OnRemoval()
    {
        base.OnRemoval();
    }
}



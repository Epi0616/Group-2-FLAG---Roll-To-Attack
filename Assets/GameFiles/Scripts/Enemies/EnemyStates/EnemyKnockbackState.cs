using UnityEngine;

public class EnemyKnockbackState : EnemyBaseState
{
    private float force;

    public EnemyKnockbackState(float force)
    {
        this.force = force;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);

        ApplyKnockback();
    }

    private void ApplyKnockback()
    {
        Vector3 targetVector = (enemy.transform.position - enemy.playerReference.transform.position);
        Vector3 targetDirection = targetVector.normalized;
        enemy.rb.AddForce(targetDirection * (force * enemy.knockbackWeightModifier), ForceMode.Impulse);
    }

    public override void UpdateState()
    {
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, Vector3.zero, 1.1f * Time.deltaTime);
        if (enemy.rb.linearVelocity.magnitude <= 2f)
        {
            enemy.ChangeState(new EnemyMoveState());
        }
    }
}

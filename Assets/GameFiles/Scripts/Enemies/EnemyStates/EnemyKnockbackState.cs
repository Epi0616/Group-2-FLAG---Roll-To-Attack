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
        enemy.rb.useGravity = true;
        enemy.rb.isKinematic = false;
        ApplyKnockback();
    }

    private void ApplyKnockback()
    {
        Vector3 targetVector = (enemy.transform.position - enemy.playerReference.transform.position);
        Vector3 targetDirection = targetVector.normalized;
        enemy.rb.AddForce(targetDirection * (force * enemy.knockbackWeightModifier * 2), ForceMode.Impulse);
    }

    public override void UpdateState()
    {
        //enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, Vector3.zero, 1.1f * Time.deltaTime);
        if (enemy.rb.linearVelocity.magnitude <= 2f)
        {
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    public override void ExitState()
    {
        enemy.rb.linearVelocity = Vector3.zero;    
        enemy.rb.useGravity = false;
        enemy.rb.isKinematic = true;
        enemy.enemyAgent.Warp(enemy.transform.position);
    }
}

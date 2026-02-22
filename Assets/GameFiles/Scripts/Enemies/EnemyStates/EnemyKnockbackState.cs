using UnityEngine;

public class EnemyKnockbackState : EnemyBaseState
{
    private float force;
    private Vector3 origin;
    private float minKnockback = 0.5f;
    private float knockbackTimer;

    public EnemyKnockbackState(Vector3 origin, float force)
    {
        this.force = force;
        this.origin = origin;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
        // Move to Using Physics controlled Movement
        enemy.rb.useGravity = true;
        enemy.rb.isKinematic = false;      
        enemy.rb.linearDamping = 3f;
        enemy.isKnockedBack = true;

        knockbackTimer = 0f;

        ApplyKnockback();
    }

    private void ApplyKnockback()
    {
        enemy.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (enemy.transform.position - origin);
        targetVector.y = 0f;
        Vector3 targetDirection = targetVector.normalized;
        enemy.rb.AddForce(targetDirection * ((force * enemy.knockbackWeightModifier) * 5), ForceMode.VelocityChange);
    }

    public override void UpdateState()
    {
        knockbackTimer += Time.deltaTime;
        //enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, Vector3.zero, 1.1f * Time.deltaTime);
        //Debug.Log(enemy.rb.linearVelocity.magnitude);

        // Check for Enemy slowing enough after knockback to return to moving
        if (enemy.rb.linearVelocity.magnitude <= 2f && knockbackTimer >= minKnockback)
        {          
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    public override void ExitState()
    {
        // Reset all RigidBody controlled Movement to allow NavMesh to take over again
        enemy.rb.linearDamping = 0f;
        enemy.rb.linearVelocity = Vector3.zero;    
        enemy.rb.useGravity = false;
        enemy.rb.isKinematic = true;

        enemy.isKnockedBack = false;

        enemy.enemyAgent.updatePosition = true;
        enemy.enemyAgent.updateRotation = true;

        // Account for moving without NavMesh so AI doesn't get lost
        enemy.enemyAgent.Warp(enemy.transform.position);
    }
}

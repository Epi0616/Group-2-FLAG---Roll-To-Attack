using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockbackState : EnemyBaseState
{
    protected float force;
    protected Vector3 origin;
    protected float minKnockback = 0.5f;
    protected float knockbackTimer;

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
        
        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0.3f;
        enemy.rb.AddForce(targetDirection * ((force * enemy.knockbackWeightModifierStat.GetFinalValue()) * 10f), ForceMode.VelocityChange);
    }

    public override void UpdateState()
    {
        
        knockbackTimer += Time.deltaTime;
       
        // Check for Enemy slowing enough after knockback to return to moving
        if (enemy.rb.linearVelocity.magnitude <= 2f && knockbackTimer >= minKnockback)
        {
            NavMeshHit hit;
            bool validNavMeshNode = NavMesh.SamplePosition(enemy.transform.position, out hit, 3f, NavMesh.AllAreas);
            if (validNavMeshNode)
            {
                Vector3 destinationPos = new Vector3(hit.position.x, enemy.transform.position.y, hit.position.z);
                Vector3 returntoNavMeshDirection = (destinationPos - enemy.transform.position).normalized;
                returntoNavMeshDirection.y = enemy.transform.position.y;
                enemy.rb.MovePosition(enemy.transform.position + returntoNavMeshDirection * 10f * Time.deltaTime);
                
                float distance = Vector3.Distance(enemy.transform.position, destinationPos);
                if (distance < 0.05f)
                {
                    enemy.ChangeState(new EnemyMoveState());
                }
            }
        }
    }

    public override void FixedUpdateState()
    {
        if (enemy.rb.linearVelocity.y < 0)
        {
            enemy.rb.AddForce(new Vector3(0, -1.5f, 0), ForceMode.Impulse);
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

public class EnemyGolemKnockbackState : EnemyKnockbackState
{
   
    public EnemyGolemKnockbackState(Vector3 origin, float force) : base(origin, force)
    {
        this.force = force;
        this.origin = origin;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        enemy.isKnockedBackByGolem = true;
        base.EnterState(enemy);
        
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.isKnockedBackByGolem = false;
    }
}

using UnityEngine;

public class EnemyBodySystem : EntityBodySystem
{
    public Animator animator;
    private IStunable stunable;
    private bool animatorWasPaused;
    public override void InitialiseSystem(Entity entity)
    {
        base.InitialiseSystem(entity);
        animator.SetTrigger("isSpawning");
        stunable = entity as IStunable;
    }

    public void Update()
    {
        if (stunable.isStunned)
        {
            animator.speed = 0;
            animatorWasPaused = true;
        }
        else
        {
            if (animatorWasPaused)
            {
                ForceIdleAnim();
                animatorWasPaused = false;
            }
            animator.speed = 1;
        }
    }

    public void OnEnable()
    {
        animator.SetTrigger("isSpawning");
    }
    public void UpdateAnimatorBoolParameter(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void UpdateAnimatorSpeedParamter(float speed)
    {
        animator.SetFloat("MoveSpeed", speed);
    }

    public void TriggerAnimatorAttackParamter()
    {
        Debug.Log("Triggered");
        animator.SetTrigger("isAttacking");
    }

    public void TriggerAnimatorDeathParameter()
    {
        animator.SetTrigger("isDead");
    }

    public void ForceIdleAnim()
    {
        animator.Play("Idle");
    }

}

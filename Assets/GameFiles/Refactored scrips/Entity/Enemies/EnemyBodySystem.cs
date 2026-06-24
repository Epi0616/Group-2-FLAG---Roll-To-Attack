using UnityEngine;

public class EnemyBodySystem : EntityBodySystem
{
    public Animator animator;
    
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
        animator.SetTrigger("isAttacking 0");
    }

}

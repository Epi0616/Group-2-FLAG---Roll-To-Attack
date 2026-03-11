using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerBaseAttackState : PlayerMovementState
{
    protected Color myColor = Color.red;
    protected float myRadius;
    protected bool attacked;

    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        myRadius = player.baseRadiusSize.GetFinalValue();
        attacked = false;
    }
    public override void FixedUpdateState()
    {
        Vector3 targetVelocity = moveDirection * player.moveSpeed.GetFinalValue();

        if (!player.isGrounded)
        {
            targetVelocity.y = player.rb.linearVelocity.y - player.impactSpeed.GetFinalValue();
        }
        
        player.rb.linearVelocity = targetVelocity;

        if (!player.isGrounded) { return; }
        ImpactGround();
    }
    protected virtual void ImpactGround()
    {
        if (attacked) { return; }
        attacked = true;

        float magnitude = player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue() * 2;
        player.AddScreenShake(magnitude);
        Collider[] colliders = Physics.OverlapSphere(player.rb.position, myRadius);
        Attack(colliders);
        ResetAttackModifiers();
        player.SwitchState(new PlayerMovementState());
    }

    protected virtual void Attack(Collider[] colliders)
    {
        List<GameObject> Enemies = new();
        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }

            if (collider.gameObject.CompareTag("Enemy"))
            {
                Enemies.Add(collider.gameObject);
            }
        }

        foreach (var Enemy in Enemies)
        {
            if (Enemy == null) continue;
            CustomAttack(Enemy);
            ApplyKnockback(Enemy);
        }
        CustomDisplayAttack();
    }

    protected void ApplyKnockback(GameObject Enemy)
    {
        if (player.impactSpeed.GetFinalValue() > player.impactSpeed.GetBaseValue())
        { 
            EnemyStateController enemy = Enemy.GetComponent<EnemyStateController>();
            float knockbackForce = player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue();
            enemy.OnTakeKnockback(player.transform.position, knockbackForce * 3);
        }
    }

    private void ResetAttackModifiers()
    {
        player.jumpHeight.ResetModifiers();
        player.impactSpeed.ResetModifiers();
        player.baseRadiusSize.ResetModifiers();
    }

    protected virtual void CustomDisplayAttack() { }
    protected virtual void CustomAttack(GameObject Enemy) { }
    //check for attack action pressed needs to do nothing so you cant attack while in the process of performing the current attack
}


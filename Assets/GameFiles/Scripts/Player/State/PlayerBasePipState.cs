using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerBasePipState : PlayerMovementState
{
    protected float myRadius;
    protected float myRadiusMultiplier;
    protected bool attacked;

    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        myRadius = player.baseRadiusSize * myRadiusMultiplier;
        attacked = false;
    }
    public override void FixedUpdateState()
    {
        Vector3 targetVelocity = moveDirection * player.moveSpeed;

        if (!player.isGrounded)
        { 
            targetVelocity.y = player.rb.linearVelocity.y - player.impactSpeed;
        }
        
        player.rb.linearVelocity = targetVelocity;

        if (!player.isGrounded) { return; }
        ImpactGround();
    }
    protected virtual void ImpactGround()
    {
        if (attacked) { return; }
        attacked = true;

        Debug.Log(myRadius);
        Collider[] colliders = Physics.OverlapSphere(player.rb.position, myRadius);
        Attack(colliders);
        player.SwitchState(new PlayerMovementState());
    }

    protected void Attack(Collider[] colliders)
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
            CustomAttack(Enemy);
        }
        CustomDisplayAttack();
    }

    protected virtual void CustomDisplayAttack() { }
    protected virtual void CustomAttack(GameObject Enemy) { }
    //check for attack action pressed needs to do nothing so you cant attack while in the process of performing the current attack
    protected override void CheckForAttackActionPressed() { }

}


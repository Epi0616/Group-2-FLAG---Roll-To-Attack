using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTwoPipState : PlayerMovementState
{
    private float myRadius;
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        myRadius = player.baseRadiusSize * 1.5f;
    }
    public override void UpdateState()
    {
        base.UpdateState();
    }
    public override void FixedUpdateState()
    {
        Vector3 targetVelocity = moveDirection * player.moveSpeed;
        targetVelocity.y = player.rb.linearVelocity.y - player.impactSpeed;
        player.rb.linearVelocity = targetVelocity;

        if (!player.isGrounded) { return; }
        ImpactGround();
    }
    private void ImpactGround()
    {
        Debug.Log(myRadius);
        Collider[] colliders = Physics.OverlapSphere(player.rb.position, myRadius);
        Attack(colliders);
        player.SwitchState(new PlayerMovementState());
    }

    private void Attack(Collider[] colliders)
    {
        List<GameObject> Enemies = new List<GameObject>();
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
            Enemy.GetComponent<EnemyBaseClass>().OnTakeDamage(30);
        }
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius);
    }

    protected override void CheckForAttackActionPressed()
    {
        //do nothing
    }
}


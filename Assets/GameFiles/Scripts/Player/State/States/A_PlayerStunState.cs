using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerStunState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 1.5f;
        base.EnterState(player);

        myColor = Color.lightBlue;
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        EnemyStateController enemyTempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        enemyTempScriptAccess.OnTakeDamage(30);
        enemyTempScriptAccess.OnRecieveEffect(new FragileEffect(2f, 1.5f, "Fragile"));
        enemyTempScriptAccess.OnStunned();
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}
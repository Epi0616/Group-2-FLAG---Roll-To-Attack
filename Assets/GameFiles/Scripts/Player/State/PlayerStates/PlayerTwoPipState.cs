using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTwoPipState : PlayerBasePipState
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
        enemyTempScriptAccess.OnStunned();
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}
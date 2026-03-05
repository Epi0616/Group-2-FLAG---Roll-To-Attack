using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerBasicState : PlayerBasePipState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 1f;
        base.EnterState(player);

        myColor = Color.red;
    }
    protected override void CustomAttack(GameObject enemy)
    {
        EnemyStateController tempControllerReference = enemy.GetComponent<EnemyStateController>();
        tempControllerReference.OnTakeDamage(500);
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

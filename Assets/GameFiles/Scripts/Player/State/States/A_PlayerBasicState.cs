using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerBasicState : PlayerBaseAttackState
{
    int critDamage;
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);

        myColor = Color.red;

        critDamage = Random.Range(1, 11);
    }
    protected override void CustomAttack(GameObject enemy)
    {
        EnemyStateController tempControllerReference = enemy.GetComponent<EnemyStateController>();
        if (critDamage <= 2)
        {
            tempControllerReference.OnTakeDamage(36, myColor);
        }
        else
        {
            tempControllerReference.OnTakeDamage(32, myColor);
        }
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

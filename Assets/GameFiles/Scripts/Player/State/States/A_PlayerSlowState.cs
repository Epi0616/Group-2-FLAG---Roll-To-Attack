using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerSlowState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 2f;
        base.EnterState(player);

        myColor = Color.white;
    }
    protected override void CustomAttack(GameObject enemy)
    {
        EnemyStateController tempControllerReference = enemy.GetComponent<EnemyStateController>();
        tempControllerReference.OnTakeDamage(30);
        tempControllerReference.OnRecieveEffect(new SlowEffect(5, 0.75f, "Slowed"), myColor);
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerSlowState : PlayerBasePipState
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
        tempControllerReference.OnRecieveEffect(new SlowEffect(5, 0.75f, "Slowed"));
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerRocketState : PlayerBasePipState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 1f;
        base.EnterState(player);

        myColor = Color.orange;
    }
    protected override void CustomAttack(GameObject enemy)
    {
        EnemyStateController tempControllerReference = enemy.GetComponent<EnemyStateController>();
        player.CreateRockets(tempControllerReference);
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

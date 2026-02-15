using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerOnePipState : PlayerBasePipState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 1f;
        base.EnterState(player);

        myColor = Color.red;
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        Enemy.GetComponent<EnemyStateController>().OnTakeDamage(50);
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

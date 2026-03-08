using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerWeakenState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 2f;
        base.EnterState(player);

        myColor = Color.darkMagenta;
    }
    protected override void CustomAttack(GameObject enemy)
    {
        EnemyStateController tempControllerReference = enemy.GetComponent<EnemyStateController>();
        tempControllerReference.OnTakeDamage(5);
        tempControllerReference.OnRecieveEffect(new WeakenEffect(5, 1, "Weakened"), myColor);
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

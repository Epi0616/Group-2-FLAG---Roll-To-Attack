using UnityEngine;

public class A_PlayerKnockbackState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 3f;
        base.EnterState(player);

        myColor = Color.darkGoldenRod;
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        EnemyStateController tempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        tempScriptAccess.OnTakeDamage(35, myColor);
        tempScriptAccess.OnTakeKnockback(player.transform.position, 5);
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

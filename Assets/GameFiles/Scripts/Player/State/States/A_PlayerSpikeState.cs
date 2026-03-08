using UnityEngine;

public class A_PlayerSpikeState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 2.5f;
        base.EnterState(player);

        myColor = Color.silver;
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        EnemyStateController enemyTempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        enemyTempScriptAccess.OnTakeDamage(25);
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
        player.attackSystem.CreateFourPipSpikesInOrbit();
    }
}

using UnityEngine;

public class A_PlayerPoisonState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 2f;
        base.EnterState(player);

        myColor = Color.green;
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        //Enemy.GetComponent<EnemyStateController>().OnTakeDamage(15, myColor);
    }

    protected override void CustomDisplayAttack()
    {
        //player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);

        GameObject poisionField = player.attackSystem.InstantiateObejct(player.attackSystem.poisonImpactField, player.rb.position);
        poisionField.GetComponent<PoisionImpactField>().adjustObjectSizeAndRotation(myRadius);
    }
}

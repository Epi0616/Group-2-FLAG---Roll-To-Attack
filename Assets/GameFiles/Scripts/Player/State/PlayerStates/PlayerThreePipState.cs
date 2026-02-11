using UnityEngine;

public class PlayerThreePipState : PlayerBasePipState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 2f;
        base.EnterState(player);
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        Enemy.GetComponent<EnemyBaseClass>().OnTakeDamage(15);
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius);

        GameObject poisionField = player.InstantiateObejct(player.poisonImpactField, player.rb.position);
        poisionField.GetComponent<PoisionImpactField>().adjustObjectSizeAndRotation(myRadius);
    }
}

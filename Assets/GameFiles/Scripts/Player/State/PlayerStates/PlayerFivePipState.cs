using UnityEngine;

public class PlayerFivePipState : PlayerBasePipState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 3f;
        base.EnterState(player);
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        EnemyStateController tempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        tempScriptAccess.OnTakeDamage(35);
        tempScriptAccess.OnTakeKnockback(5);
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius);
    }
}

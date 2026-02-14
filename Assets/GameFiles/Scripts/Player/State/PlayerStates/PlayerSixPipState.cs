using Unity.VisualScripting;
using UnityEngine;

public class PlayerSixPipState : PlayerBasePipState
{
    private bool gameFrozen;
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 3.5f;
        base.EnterState(player);

        gameFrozen = false;
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        EnemyStateController enemyTempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        enemyTempScriptAccess.OnTakeDamage(30);

        gameFrozen = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!gameFrozen) { return; }
        ApproachGameFreeze();
    }

    protected override void ImpactGround()
    {
        if (attacked) { return; }
        attacked = true;

        Debug.Log(myRadius);
        Collider[] colliders = Physics.OverlapSphere(player.rb.position, myRadius);
        Attack(colliders);
    }

    protected override void CustomDisplayAttack()
    {
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius);
    }

    protected void ApproachGameFreeze()
    {
        Debug.Log("freezing");
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.2f, 2f * Time.unscaledDeltaTime);
    }
}

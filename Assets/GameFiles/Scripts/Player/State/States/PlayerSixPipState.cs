using Unity.VisualScripting;
using UnityEngine;

public class PlayerSixPipState : PlayerBasePipState
{
    private bool gameFrozen;
    public float timer;
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 3.5f;
        base.EnterState(player);

        gameFrozen = true;
        myColor = Color.white;
    }
    protected override void CustomAttack(GameObject Enemy)
    {
        EnemyStateController enemyTempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        enemyTempScriptAccess.OnTakeDamage(30);

        timer = 3f;
        Enemy.transform.position += new Vector3(player.transform.position.x - Enemy.transform.position.x, 0, player.transform.position.z - Enemy.transform.position.z).normalized * 10f;
        player.transform.position = new Vector3(0, 0, 0);
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            gameFrozen = false;
            timer = 0;
        }
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
        player.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }

    protected void ApproachGameFreeze()
    {
        Debug.Log("freezing");
        //Time.timeScale = 0;
        //Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.2f, 2f * Time.unscaledDeltaTime);
    }
}

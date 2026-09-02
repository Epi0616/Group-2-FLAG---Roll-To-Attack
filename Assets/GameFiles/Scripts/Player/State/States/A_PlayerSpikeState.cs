using UnityEngine;

public class A_PlayerSpikeState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        impactSounds = player.playerSpikeSounds;
        hitSounds = player.hitPlayerSpikeSounds;
        myColor = Color.silver;
    }
    protected override void PlayImpactSound()
    {
        if (player.impactSpeed.GetFinalValue() > player.impactSpeed.GetBaseValue())
        {
            //float volumePercent = Mathf.Clamp01(player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue() - 1);
            //AudioManager.instance.PlayRandomSoundClip(player.playerHeavyAttackSounds, new Vector3(0, 0, 0), 0.2f);
        }

        //AudioManager.instance.PlayRandomSoundClip(impactSounds, new Vector3(0, 0, 0), 0.7f);
    }

    protected override void CustomAttack(GameObject Enemy)
    {
        EnemyStateController enemyTempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        //enemyTempScriptAccess.OnTakeDamage(25, myColor);
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
        player.attackSystem.CreateFourPipSpikesInOrbit();
    }
}

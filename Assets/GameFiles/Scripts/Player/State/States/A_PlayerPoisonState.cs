using UnityEngine;

public class A_PlayerPoisonState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        impactSounds = player.playerPoisonSounds;
        hitSounds = player.hitPlayerPoisonSounds;
        myColor = Color.green;
    }
    protected override void PlayImpactSound()
    {
        if (player.impactSpeed.GetFinalValue() > player.impactSpeed.GetBaseValue())
        {
            //float volumePercent = Mathf.Clamp01(player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue() - 1);
            AudioManager.instance.PlayRandomSoundClip(player.playerHeavyAttackSounds, new Vector3(0, 0, 0), 0.2f);
        }

        AudioManager.instance.PlayRandomSoundClip(impactSounds, new Vector3(0, 0, 0), 0.4f);
    }

    protected override void CustomAttack(GameObject Enemy)
    {
        //Enemy.GetComponent<EnemyStateController>().OnTakeDamage(15, myColor);
    }

    protected override void CustomDisplayAttack()
    {
        //player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);

        GameObject poisionField = player.attackSystem.InstantiateObejct(player.attackSystem.poisonImpactField, player.rb.position);
        poisionField.GetComponent<PoisionImpactField>().Initialize(myRadius);
    }
}

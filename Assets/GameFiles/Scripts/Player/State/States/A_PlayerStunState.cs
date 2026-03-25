using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerStunState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        impactSounds = player.playerFreezeSounds;
        myColor = Color.lightBlue;
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
        EnemyStateController enemyTempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        enemyTempScriptAccess.OnTakeDamage(30, myColor);
        enemyTempScriptAccess.OnRecieveEffect(new FragileEffect(2f, 2.0f, "Frozen"), myColor);
        enemyTempScriptAccess.OnStunned();
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}
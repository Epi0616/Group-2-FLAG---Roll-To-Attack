using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerRocketState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        impactSounds = player.playerRocketSounds;
        hitSounds = player.hitPlayerRocketSounds;
        myColor = Color.orange;
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

    protected override void CustomAttack(GameObject enemy)
    {
        base.CustomAttack(enemy);
        EnemyStateController tempControllerReference = enemy.GetComponent<EnemyStateController>();
        player.attackSystem.CreateRockets(tempControllerReference);
        //needs polishing as currently every enemy in range will be targeted by a rocket, but ideally it should only target one enemy and if there are multiple enemies in range, it should target the closest one.
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

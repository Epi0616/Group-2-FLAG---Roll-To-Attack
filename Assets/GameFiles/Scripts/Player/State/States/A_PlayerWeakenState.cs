using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerWeakenState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        impactSounds = player.playerWeakenSounds;
        hitSounds = player.hitPlayerWeakenSounds;
        myColor = Color.darkMagenta;
    }
    protected override void PlayImpactSound()
    {
        if (player.impactSpeed.GetFinalValue() > player.impactSpeed.GetBaseValue())
        {
            //float volumePercent = Mathf.Clamp01(player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue() - 1);
            AudioManager.instance.PlayRandomSoundClip(player.playerHeavyAttackSounds, new Vector3(0, 0, 0), 0.2f);
        }

        AudioManager.instance.PlayRandomSoundClip(impactSounds, new Vector3(0, 0, 0), 1f);
    }

    protected override void CustomAttack(GameObject enemy)
    {
        EnemyStateController enemyTempScriptAccess = enemy.GetComponent<EnemyStateController>();
        enemyTempScriptAccess.OnTakeDamage(5, myColor);
        //tempControllerReference.OnRecieveEffect(new WeakenEffect(5, 1, player.weakenedText.GetLocalizedString()), myColor);
        enemyTempScriptAccess.OnRecieveEffect(new ActiveStatusEffect(new WeakenStatus(1.0f, player.weakenedText.GetLocalizedString()),
                new List<BaseCondition> { new DurationCondition(true, 5.0f) }), myColor);
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

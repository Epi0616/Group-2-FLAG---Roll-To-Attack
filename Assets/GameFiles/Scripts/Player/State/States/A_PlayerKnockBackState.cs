using System.Collections.Generic;
using UnityEngine;

public class A_PlayerKnockbackState : PlayerBaseAttackState
{
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        impactSounds = player.playerKnockbackSounds;
        hitSounds = player.hitPlayerKnockbackSounds;
        myColor = Color.darkGoldenRod;
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
        EnemyStateController tempScriptAccess = Enemy.GetComponent<EnemyStateController>();
        tempScriptAccess.OnTakeDamage(35, myColor);
        //tempScriptAccess.OnTakeKnockback(new Vector3(player.transform.position.x, player.transform.position.y + 15, player.transform.position.z), 5);
        if (tempScriptAccess.isSpawning) { return; }
        tempScriptAccess.OnRecieveEffect(new ActiveStatusEffect(new KnockbackEffect(new Vector3(player.transform.position.x, player.transform.position.y + 15, player.transform.position.z), 5f),
                new List<BaseCondition> { new GroundedCondition(true, tempScriptAccess), new DurationCondition(true, 0.75f), new NavMeshReturnCondition(false, tempScriptAccess) }));
    }

    protected override void CustomDisplayAttack()
    {
        player.attackSystem.impactField.GetComponent<ImpactField>().ShowOnPlayer(player.rb.position, myRadius, myColor);
    }
}

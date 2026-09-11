using UnityEngine;

public class SlimeHealthSystem : EnemyHealthSystem
{
    public override void OnDeath()
    {
        if (isDead) { return; }
        base.OnDeath();
        isDead = true;

        if (ownerEntity is IActionable temp)
        {
            temp.actionController.InterruptInterruptableActions();
        }

        //OwnerEntity.statusSystem.currentActiveStatusEffects.Clear();

        if (ownerEntity is IAnimated animated)
        {
            float deathAnimationTime = 2f;
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Death, 0, MixerType.main, 0.2f, deathAnimationTime);
            StartCoroutine(DelayedDeath(deathAnimationTime, animated));
        }
        else
        {
            EnemyDeath();
        }
    }
}

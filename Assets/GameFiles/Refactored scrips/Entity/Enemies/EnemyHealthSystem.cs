using UnityEngine;
using System;
using System.Collections;

public class EnemyHealthSystem : EntityHealthSystem
{
    public static event Action EnemyHasDied;
    
    public override void OnTakeDamage(int damageAmount, DamageType type)
    {
        currentHealth -= damageAmount;
        RunTimeStatTracker.totalDamageDealt += damageAmount;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        if (isDead) { return; }
        base.OnDeath();        
        isDead = true;
        
        if (OwnerEntity is IActionable temp)
        {
            temp.actionController.InterruptInterruptableActions();
        }

        //OwnerEntity.statusSystem.currentActiveStatusEffects.Clear();

        if (OwnerEntity is IAnimated animated)
        {
            float deathAnimationTime = 2;
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Death, 0, MixerType.main, 0.2f, deathAnimationTime);
            StartCoroutine(DelayedDeath(deathAnimationTime, animated));
        }
        else 
        {
            EnemyDeath();
        }
    }

    private IEnumerator DelayedDeath(float delayTime, IAnimated animated)
    {
        yield return new WaitForSeconds(delayTime);

        //Debug.Log("wait complete");
        //animated.animationManager.EndCurrentAnimation(MixerType.main);
        //animated.animationManager.EndCurrentAnimation(MixerType.complimentary);
        EnemyDeath();
    }

    private void EnemyDeath()
    {
        OwnerEntity.bodySystem.RemoveAllShaders();
        try
        {
            if (OwnerEntity is IWaveEnemy enemy)
            {
                if (enemy.isWaveEnemy)
                {
                    EnemyHasDied?.Invoke();
                }
            }

            ObjectPoolManager.ReturnObjectToPool(OwnerEntity.gameObject, 0);
        }
        catch
        {
            Destroy(OwnerEntity.gameObject);
        }
    }
}

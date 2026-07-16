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
        base.OnDeath();
        if (isDead) { return; }
        isDead = true;
        
        if (OwnerEntity is IActionable temp)
        {
            temp.actionController.InterruptAllActive();
        }

        OwnerEntity.statusSystem.currentActiveStatusEffects.Clear();

        if (OwnerEntity is IAnimated animated)
        {
            float deathAnimationTime = 2;
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Death, 0, 0.2f, deathAnimationTime);
            StartCoroutine(DelayedDeath(deathAnimationTime));

        }
        else 
        {
            EnemyDeath();
        }
    }

    public void HandleDeathAfterAnimation(float delayTime)
    {
        StartCoroutine(DelayedDeath(delayTime));
    }

    private IEnumerator DelayedDeath(float delayTime)
    { 
        yield return new WaitForSeconds(delayTime);
        EnemyDeath();
    }

    private void EnemyDeath()
    {
        try
        {
            EnemyHasDied?.Invoke();
            ObjectPoolManager.ReturnObjectToPool(OwnerEntity.gameObject, 0);
        }
        catch
        {
            Destroy(OwnerEntity.gameObject);
        }
    }
}

using UnityEngine;
using System;
using System.Collections;

public class EnemyHealthSystem : EntityHealthSystem
{
    public static event Action EnemyHasDied;

    [SerializeField] private float deathAnimationTime = 0.5f;

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
        
        if (ownerEntity is IActionable temp)
        {
            temp.actionController.InterruptInterruptableActions();
        }

        //OwnerEntity.statusSystem.currentActiveStatusEffects.Clear();

        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Death, 0, MixerType.main, 0.2f, deathAnimationTime);
            StartCoroutine(DelayedDeath(deathAnimationTime, animated));
        }
        else 
        {
            EnemyDeath();
        }
    }

    protected IEnumerator DelayedDeath(float delayTime, IAnimated animated)
    {
        yield return new WaitForSeconds(delayTime);

        //Debug.Log("wait complete");
        //animated.animationManager.EndCurrentAnimation(MixerType.main);
        //animated.animationManager.EndCurrentAnimation(MixerType.complimentary);
        EnemyDeath();
    }

    protected void EnemyDeath()
    {
        
        try
        {
            if (ownerEntity is IWaveEnemy enemy)
            {
                if (enemy.isWaveEnemy)
                {
                    EnemyHasDied?.Invoke();
                }
            }
            ownerEntity.bodySystem.RemoveAllShaders();
            ObjectPoolManager.ReturnObjectToPool(ownerEntity.gameObject, 0);
        }
        catch
        {
            ownerEntity.bodySystem.RemoveAllShaders();
            Destroy(ownerEntity.gameObject);
        }

    }
}

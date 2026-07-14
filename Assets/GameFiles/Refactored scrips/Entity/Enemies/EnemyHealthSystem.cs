using UnityEngine;
using System;
using System.Collections;

public class EnemyHealthSystem : EntityHealthSystem
{
    public static event Action EnemyHasDied;
    public event Action LocalEnemyDeathEvent;

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
            animated.animationManager.PlayAnimation(EnemyAnimations.Death, 0.5f);
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
            LocalEnemyDeathEvent?.Invoke();
            ObjectPoolManager.ReturnObjectToPool(OwnerEntity.gameObject, 0);
        }
        catch
        {
            Destroy(OwnerEntity.gameObject);
        }
    }
}

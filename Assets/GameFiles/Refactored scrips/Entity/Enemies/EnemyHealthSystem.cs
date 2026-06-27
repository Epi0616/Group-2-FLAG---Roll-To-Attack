using UnityEngine;
using System;

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

        if (OwnerEntity.bodySystem is EnemyBodySystem temp2)
        {
            temp2.TriggerAnimatorDeathParameter();
        }

        OwnerEntity.statusSystem.currentActiveStatusEffects.Clear();

        try
        {
            EnemyHasDied?.Invoke();
            LocalEnemyDeathEvent?.Invoke();
            ObjectPoolManager.ReturnObjectToPool(OwnerEntity.gameObject);
        }
        catch
        {
            Destroy(OwnerEntity.gameObject);
        }
    }
}

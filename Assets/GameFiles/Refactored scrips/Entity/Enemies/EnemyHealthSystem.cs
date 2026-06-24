using UnityEngine;
using System;

public class EnemyHealthSystem : EntityHealthSystem
{
    public static event Action EnemyHasDied;

    public override void OnDeath()
    {
        base.OnDeath();
        if (isDead) { return; }
        isDead = true;
        
        if (OwnerEntity is IActionable temp)
        {
            temp.actionController.InterruptAllActive();
        }
        try
        {
            EnemyHasDied?.Invoke();
            ObjectPoolManager.ReturnObjectToPool(OwnerEntity.gameObject);
        }
        catch
        {
            Destroy(OwnerEntity.gameObject);
        }
    }
}

using UnityEngine;
using System;

public class EnemyHealthSystem : EntityHealthSystem
{
    public static event Action EnemyHasDied;

    public override void OnDeath()
    {
        base.OnDeath();
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

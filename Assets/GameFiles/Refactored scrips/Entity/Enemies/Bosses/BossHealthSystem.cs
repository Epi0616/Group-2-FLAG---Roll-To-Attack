using UnityEngine;
using System;

public class BossHealthSystem : EnemyHealthSystem
{
    private BaseBossEnemy boss;
    public override void InitialiseSystem(Entity entity)
    {
        base.InitialiseSystem(entity);
        if (!(entity is IBoss boss)) { Debug.Log("entity is not of type IBoss"); return; }
        this.boss = (BaseBossEnemy)entity;
        boss.HandleEnable();
        boss.HandleSetMilestones();
    }

    public override void OnTakeDamage(int damageAmount, DamageType type)
    {
        currentHealth -= damageAmount;
        boss.HandleUpdateHealth();
        RunTimeStatTracker.totalDamageDealt += damageAmount;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        if (isDead) return;
        boss.HandleDisable();
        base.OnDeath();
        isDead = true;
    }
}

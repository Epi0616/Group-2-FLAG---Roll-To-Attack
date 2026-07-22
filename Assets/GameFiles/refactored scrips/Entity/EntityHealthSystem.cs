using UnityEngine;
using System;

public class EntityHealthSystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }

    public int maxHealth;
    public int currentHealth;
    public bool isDead;

    public virtual void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        currentHealth = maxHealth;
    }

    public virtual void ResetSystem()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public virtual void OnTakeDamage(int damageAmount, DamageType type)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    public virtual void OnHeal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
    }
    public virtual void OnDeath()
    {
        //Debug.Log("dead");
    }
}

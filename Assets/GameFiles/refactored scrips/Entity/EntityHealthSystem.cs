using UnityEngine;
using System;

public class EntityHealthSystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }

    public Stat maxHealth;
    public int currentHealth;
    public bool isDead;

    private void Awake()
    {
        
    }

    public virtual void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        currentHealth = (int)maxHealth.GetFinalValue();
    }

    public virtual void ResetSystem()
    {
        currentHealth = (int)maxHealth.GetFinalValue();
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
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, (int)maxHealth.GetFinalValue());
    }
    public virtual void OnDeath()
    {
        OwnerEntity.bodySystem.RemoveAllShaders();
        //Debug.Log("dead");
    }
}

using UnityEngine;
using System;

public class EntityHealthSystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public static event Action EnemyHasDied;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    public bool isDead;

    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        currentHealth = maxHealth;
    }

    public void ResetSystem() { }

    public void OnTakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    public void OnHeal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
    }
    public void OnDeath()
    {
        try
        {
            EnemyHasDied?.Invoke();
            ObjectPoolManager.ReturnObjectToPool(OwnerEntity.gameObject);
        }
        catch
        { 
            Destroy(OwnerEntity.gameObject);
        }

        Debug.Log("dead");
    }
}

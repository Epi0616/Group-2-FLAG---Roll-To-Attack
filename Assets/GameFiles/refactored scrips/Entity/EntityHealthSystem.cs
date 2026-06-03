using UnityEngine;

public class EntityHealthSystem : IEntitySystem
{
    public Entity OwnerEntity { get; set; }

    private int maxHealth;
    private int currentHealth;
    public bool isDead;

    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        // Set MaxHealth
    }

    public void ResetSystem() { }

    public void OnTakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
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

    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum DamageType
{
    Normal
}

public interface IEntity
{
    public void OnTakeDamage(int amount, Color color, DamageType damageType);
    public void OnRecieveEffect(ActiveStatusEffect statusEffect);
}

public interface IGrounded
{
    public bool isGrounded { get; set; }
    public void CheckForGrounded();
}

public interface IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public void InitialiseSystem(Entity entity);
    public void ResetSystem();
}

public class TextDisplaySystem : IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        // Give the Camera Reference to the TextDisplaySystem
    }

    public void ResetSystem() { }

    public void DisplayText(string text, Color color, int fontSize)
    { 

    }

    
}

public class EntityStatusSystem : IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public List<ActiveStatusEffect> currentActiveStatusEffects = new List<ActiveStatusEffect>();

    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
    }

    public void ResetSystem() { }

    public void UpdateConditions()
    {




        // RECALCULATE STATS
        //whatever form that takes eventually
    }

    public void OnRecieveEffect(ActiveStatusEffect newStatus)
    {
        // Check if the new Status can be "stacked" if not simply reset the conditions of the current Status
        if (!newStatus.effect.isStackable)
        {
            for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
            {
                if (currentActiveStatusEffects[i].effect.type == newStatus.effect.type)
                {
                    //Debug.Log("Non Stackable Effect Found, Reseting Condition");
                    currentActiveStatusEffects[i].ResetConditionsAll();
                    return;
                }
            }
        }
        // Add it to the currentActiveStatusEffectsList and call the "effect added" function in the Status
        currentActiveStatusEffects.Add(newStatus);
        //newStatus.effect.AddEffect(OwnerEntity);

        // The Effect Display will be handled by the Entity itself not the Status System 

    }

    public int ModifyDamage(int damageAmount, DamageType damageType)
    {
        int modifiedDamageAmount = damageAmount;
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            // Call Status OnTakeDamage or equivelent trigger
            // Can return either the adjusted value and immediately update modifiedDamageAmount 
            // or return the delta and aggregate them applying it at the end to modify the dmg
            // Depends if we want each status to apply its modification in isolation or have the previous effect modifications effect the next ones
        }
            return modifiedDamageAmount;
    }

    public void RemoveEffectByType(StatusType type)
    {
        for(int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            // Simply Check the StatusType Enum against the desired type removing it if found
            if (currentActiveStatusEffects[i].effect.type == type)
            {
                currentActiveStatusEffects[i].effect.RemoveEffect();
                currentActiveStatusEffects.RemoveAt(i);
                //Debug.Log("Status Removed: " + type.ToString());
            }
        }

        // RECALCULATE STATS
        //whatever form that takes eventually

    }

    public void RecalculateStats()
    {
        // This entirely depends on where we store the Stats, if stored in here its easy to Recalculate but more difficult to use elsewhere
        // or if stored in the Entity the StatusSystem will need someway to access and update those Stats
    }

    
}

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
        currentHealth = Math.Clamp(currentHealth + healAmount, 0, maxHealth);
    }
    public void OnDeath()
    {

    }
}

public class Entity : MonoBehaviour, IEntity, IGrounded
{
    public bool isGrounded { get; set; }

    EntityHealthSystem healthSystem;
    EntityStatusSystem statusSystem;
    TextDisplaySystem textDisplaySystem;

    public void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        textDisplaySystem.DisplayText(finalDamage.ToString(), color, 20);

        healthSystem.OnTakeDamage(finalDamage);
    }
    public void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        textDisplaySystem.DisplayText(statusEffect.effect.GetEffectText(), Color.red, 20);

        statusSystem.OnRecieveEffect(statusEffect);
    }

    private void Update()
    {
        statusSystem.UpdateConditions();
    }

    public void CheckForGrounded()
    { 

    }
}
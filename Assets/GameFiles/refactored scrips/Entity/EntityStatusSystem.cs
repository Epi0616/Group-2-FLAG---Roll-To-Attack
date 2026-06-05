using System.Collections.Generic;
using UnityEngine;

public class EntityStatusSystem : MonoBehaviour , IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public List<ActiveStatusEffect> currentActiveStatusEffects = new List<ActiveStatusEffect>();

    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
    }

    public void ResetSystem()
    { 
        currentActiveStatusEffects.Clear();
        RecalculateStats();
        // RECALCULATE STATS
    }

    public void UpdateConditions()
    {
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentActiveStatusEffects[i].effect.isActive)
            {
                currentActiveStatusEffects[i].effect.UpdateEffect();
            }

            currentActiveStatusEffects[i].UpdateConditionsAll();

            if (currentActiveStatusEffects[i].conditions != null && (currentActiveStatusEffects[i].CheckForExpiration() || currentActiveStatusEffects[i].effect.toBeRemoved))
            {
                currentActiveStatusEffects[i].effect.RemoveEffect();
                currentActiveStatusEffects.RemoveAt(i);
            }
        }

        RecalculateStats();

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
        //int modifiedDamageAmount = damageAmount;
        Stat modifiedDamageAmount = new Stat(damageAmount);
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            currentActiveStatusEffects[i].effect.TriggerOnDamageEffects(ref  modifiedDamageAmount, damageType);
            
            // Call Status OnTakeDamage or equivelent trigger
            // Can return either the adjusted value and immediately update modifiedDamageAmount 
            // or return the delta and aggregate them applying it at the end to modify the dmg
            // Depends if we want each status to apply its modification in isolation or have the previous effect modifications effect the next ones
        }        

        return Mathf.FloorToInt(modifiedDamageAmount.GetFinalValue());
    }

    public void RemoveEffectByType(StatusType type)
    {
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            // Simply Check the StatusType Enum against the desired type removing it if found
            if (currentActiveStatusEffects[i].effect.type == type)
            {
                currentActiveStatusEffects[i].effect.RemoveEffect();
                currentActiveStatusEffects.RemoveAt(i);
                //Debug.Log("Status Removed: " + type.ToString());
            }
        }

        RecalculateStats();

        // RECALCULATE STATS
        //whatever form that takes eventually

    }

    public void RecalculateStats()
    {
        foreach (var stat in OwnerEntity.statList)
        {
            stat.ResetModifiers();
        }

        foreach (var status in currentActiveStatusEffects)
        {
            if (status.effect.isActive)
            {
                status.effect.ApplyStatModifierUpdates();
            }
            
        }       

        if (OwnerEntity is INavAgent ai && OwnerEntity is IMoveable mo)
        {
            ai.agent.speed = mo.movementSpeed.GetFinalValue();
        }

    }


}

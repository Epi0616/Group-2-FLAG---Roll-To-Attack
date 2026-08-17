using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStatusSystem : MonoBehaviour , IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public List<ActiveStatusEffect> currentActiveStatusEffects = new List<ActiveStatusEffect>();
    private Stat modifiedDamageAmount;
    //public int statusCount;
    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        modifiedDamageAmount = new Stat(1f);
    }

    public void ResetSystem()
    {
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            currentActiveStatusEffects[i].effect.toBeRemoved = true;           
        }
        RecalculateStats();
        // RECALCULATE STATS
    }

    public void FixedUpdate()
    {
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentActiveStatusEffects[i].effect.isActive)
            {
                currentActiveStatusEffects[i].effect.FixedUpdateEffect();
            }
        }
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
            
            if (currentActiveStatusEffects[i].conditions != null && (currentActiveStatusEffects[i].CheckForExpiration())) { currentActiveStatusEffects[i].effect.toBeRemoved = true; }
            //{
            //    //Debug.Log("Before there are: " + currentActiveStatusEffects.Count + " statuses");
            //    //Debug.Log("Effect Removed: " + currentActiveStatusEffects[i].effect.GetType().ToString());
            //    //bool isLast = true;
            //    //for (int j = currentActiveStatusEffects.Count - 1; j >= 0; j--)
            //    //{
            //    //    if (j == i)
            //    //        continue;

            //    //    if (currentActiveStatusEffects[j].effect.type == currentActiveStatusEffects[i].effect.type)
            //    //    {
            //    //        isLast = false;
            //    //        break;
            //    //    }
            //    //}
            //    //if (isLast)
            //    //{
            //    //    currentActiveStatusEffects[i].effect.LastStackEffect();
            //    //}
            //    //currentActiveStatusEffects[i].effect.RemoveEffect();
            //    //currentActiveStatusEffects.RemoveAt(i);
            //    //Debug.Log("Now there are: " + currentActiveStatusEffects.Count + " statuses");
               
            //}
        }

        RemoveStatuses();
        RecalculateStats();

        // RECALCULATE STATS
        //whatever form that takes eventually
    }

    public void RemoveStatuses()
    {        

        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (!currentActiveStatusEffects[i].effect.toBeRemoved) { continue; }

            bool isLast = !currentActiveStatusEffects.Any(effect => effect != currentActiveStatusEffects[i] && effect.effect.type == currentActiveStatusEffects[i].effect.type);

            if (isLast) { currentActiveStatusEffects[i].effect.LastStackEffect(); }

            currentActiveStatusEffects[i].effect.RemoveEffect();
            currentActiveStatusEffects.RemoveAt(i);
        }
    }

    public void OnRecieveEffect(ActiveStatusEffect newStatus)
    {
        bool isFirst = true;
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

        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentActiveStatusEffects[i].effect.type == newStatus.effect.type)
            {
                isFirst = false;
                break;
            }
        }
        // Add it to the currentActiveStatusEffectsList and call the "effect added" function in the Status
        //if (OwnerEntity == null) { Debug.Log("Owner Inside of Status System is NULL"); }
        newStatus.effect.AddEffect(OwnerEntity);
        if (isFirst)
        {
            newStatus.effect.FirstStackEffect();
        }
        foreach (BaseCondition condition in newStatus.conditions)
        {
            condition.Initialize(OwnerEntity);
        }
        currentActiveStatusEffects.Add(newStatus);
        // The Effect Display will be handled by the Entity itself not the Status System 

    }

    public int ModifyDamage(int damageAmount, DamageType damageType)
    {
        //int modifiedDamageAmount = damageAmount;
        modifiedDamageAmount.ResetModifiers();
        modifiedDamageAmount.AddAdditive(damageAmount);
        //for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        //{
        //    if (currentActiveStatusEffects[i] == null || currentActiveStatusEffects[i].effect.toBeRemoved) { continue; }
        //    currentActiveStatusEffects[i].effect.TriggerOnDamageEffects(ref modifiedDamageAmount, damageType);

        //    // Call Status OnTakeDamage or equivelent trigger
        //    // Can return either the adjusted value and immediately update modifiedDamageAmount 
        //    // or return the delta and aggregate them applying it at the end to modify the dmg
        //    // Depends if we want each status to apply its modification in isolation or have the previous effect modifications effect the next ones
        //}
        int i = 0;
        while (i < currentActiveStatusEffects.Count)
        {
            ActiveStatusEffect wewa = currentActiveStatusEffects[i];
            currentActiveStatusEffects[i].effect.TriggerOnDamageEffects(ref modifiedDamageAmount, damageType);
            if (i < currentActiveStatusEffects.Count && currentActiveStatusEffects[i] == wewa) { i++; }
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
                currentActiveStatusEffects[i].effect.toBeRemoved = true;
                //Debug.Log("Status Removed: " + type.ToString());
            }
        }

        RecalculateStats();

        // RECALCULATE STATS
        //whatever form that takes eventually

    }

    public void ResetStatusByType(StatusType type)
    {

        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            // Simply Check the StatusType Enum against the desired type reseting it if found
            if (currentActiveStatusEffects[i].effect.type == type)
            {
                currentActiveStatusEffects[i].ResetConditionsAll();
                //Debug.Log("Status Reset: " + type.ToString());
            }
        }
    }

    public bool CheckForStatusByType(StatusType type)
    {
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            // Simply Check the StatusType Enum against the desired type reseting it if found
            if (currentActiveStatusEffects[i].effect.type == type)
            {
                return true;
                //Debug.Log("Status Reset: " + type.ToString());
            }
        }
        return false;
    }

    public void RecalculateStats()
    {  
        if (OwnerEntity == null) { return; }

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
        //statusCount = currentActiveStatusEffects.Count;
    }

    public bool CheckForMovementBlockersStatus()
    {
       // bool result = false;
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentActiveStatusEffects[i].effect.preventsMovement) { return true; }
        }
        
        return false;
    }

    public bool CheckForActionBlockersStatus()
    {
        
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentActiveStatusEffects[i].effect.preventsAction) { return true; }
        }
        return false;
    }

    public bool CheckForDisplacementStatus()
    {
        bool result = false;
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentActiveStatusEffects[i].effect.isDisplacing) { result = true; }
        }
        return result;
    }

    public bool CheckForStunnedStatus()
    {
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentActiveStatusEffects[i].effect is BaseStunEffect) { return true; }
        }
        return false;
    }

}

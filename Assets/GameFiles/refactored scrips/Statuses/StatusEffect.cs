using UnityEngine;

public enum StatusType
{
    Freeze, Knockback, Weak, Slow, Stun, Poison, Speed, Crumbling, Shield, Confused
}


//currently only works with enemy, however this can be changed by making a parent class for enemy and player and then using that reference in the ApplyStatModifier() function
public abstract class StatusEffect
{

    protected string effectText;
    protected Color effectColour;

    public bool isDisplacing;
    public bool preventsMovement;
    public bool preventsAction;

    public bool isStackable = false; // if true new instances can be added if not the duration of a current instance resets
    public bool isActive = true;
    //public bool isUnique = false; // if true will prevent any new instances being added if one already exists

    public bool toBeRemoved = false;

    public StatusType type;

    public Entity entityRef;

    public void AddEffect(Entity entity)
    {
        entityRef = entity;
        if (entity == null)
        {
            Debug.Log("Passed Entity is NULL");
        }
        else if (entityRef == null)
        {
            Debug.Log("Held Reference is NULL");
        }
        OnApplication();
    }

    public void UpdateEffect()
    {
        OnUpdate();
    }

    public void FixedUpdateEffect()
    {
        OnFixedUpdate();
    }

    public void RemoveEffect()
    {
        OnRemoval();
    }

    public void ApplyStatModifierUpdates()
    {
        ApplyStatModifier();
    }

    public void TriggerOnDamageEffects(ref Stat damage, DamageType type)
    {
        ApplyOnDamageEffects(ref damage, type);
    }

    public string GetEffectText()
    {
        return effectText;
    }
    public Color GetEffectColour()
    {
        return effectColour;
    }

    protected virtual void OnApplication() { }
    protected virtual void OnUpdate() { }

    protected virtual void OnFixedUpdate() { }
    protected virtual void OnRemoval() { }

    protected virtual void ApplyStatModifier() { }
    protected virtual void ApplyOnDamageEffects(ref Stat damage, DamageType type) { }
}


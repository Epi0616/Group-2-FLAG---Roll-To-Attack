using UnityEngine;

public enum StatusType
{
    Freeze, Knockback, Weak, Slow
}


//currently only works with enemy, however this can be changed by making a parent class for enemy and player and then using that reference in the ApplyStatModifier() function
public abstract class StatusEffect
{
    //protected float timer;

    protected string effectText;

    public bool isDisplacing;
    public bool preventsMovement;
    public bool preventsAttack;
    public bool disablesAI;
    public bool isStackable = false; // if true new instances can be added if not the duration of a current instance resets

    //public bool isUnique = false; // if true will prevent any new instances being added if one already exists

    public StatusType type;

    protected EnemyStateController enemyRef;

    public void AddEffect(EnemyStateController enemy)
    {
        enemyRef = enemy;
        OnApplication();
    }

    public void UpdateEffect()
    {
        OnUpdate();
    }

    public void RemoveEffect()
    {
        OnRemoval();
    }

    public void ApplyStatModifierUpdates()
    {
        ApplyStatModifier();
    }

    /*
    public virtual void TimerUpdate()
    {
        timer -= Time.deltaTime;
    }
    public bool IsExpired()
    {
        if (timer > 0) return false;
        return true;
    }
    */

    public string GetEffectText()
    {
        return effectText;
    }

    protected virtual void OnApplication() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnRemoval() { }

    protected virtual void ApplyStatModifier() { }
}


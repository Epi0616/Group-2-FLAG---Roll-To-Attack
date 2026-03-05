using UnityEngine;

//currently only works with enemy, however this can be changed by making a parent class for enemy and player and then using that reference in the ApplyStatModifier() function
public abstract class StatusEffect
{
    protected float timer;

    public virtual void Update()
    {
        timer -= Time.deltaTime;
    }
    public bool IsExpired()
    {
        if (timer > 0) return false;
        return true;
    }

    public abstract void ApplyStatModifier(EnemyStateController enemy);
}

using UnityEngine;

public class WeakenStatus : StatusEffect
{
    protected float weakMultiplier;

    public WeakenStatus(float weakMultiplier, string effectText)
    {
        type = StatusType.Weak;
        this.weakMultiplier = weakMultiplier;
        this.effectText = effectText;
        this.effectColour = Color.darkMagenta;
        isStackable = true;
    }
    /*
    protected override void ApplyStatModifier()
    {
        enemyRef.damageTakenModifierStat.AddMultiplierFlat(weakMultiplier);
    }
    */

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        //damage.AddMultiplierFlat(weakMultiplier);
       // Debug.Log("Weaken Being Applied");
        if (type == DamageType.Weaken)
        {
           // Debug.Log("Damage Type is Weaken");
            return;
        }

        // Debug.Log("New Weaken OnTakeDamage");
        int appliedDamage = (int)(damage.GetFinalValue() * (weakMultiplier - 1));
        if (appliedDamage < 1)
        {
            appliedDamage = 1;
        }
        entityRef.OnTakeDamage(1, effectColour, DamageType.Weaken);
        
    }

    
    //protected override void OnApplication()
    //{
    //    base.OnApplication();
        
    //}

    protected override void OnFirstStackApplication()
    {
        //entityRef.bodySystem.ApplyWeakenShader(effectColour);
        entityRef.bodySystem.ApplyShader(effectColour * 3, 0.5f, ShaderType.Weakened);
    }

    //protected override void OnUpdate()
    //{
        
    //}

    //protected override void OnRemoval()
    //{      
    //    base.OnRemoval();
    //}

    protected override void OnLastStackRemoval()
    {
        entityRef.bodySystem.RemoveShader(0.25f, ShaderType.Weakened);
    }

    public override StatusEffect Clone()
    {
        return new WeakenStatus(weakMultiplier, effectText);
    }
}

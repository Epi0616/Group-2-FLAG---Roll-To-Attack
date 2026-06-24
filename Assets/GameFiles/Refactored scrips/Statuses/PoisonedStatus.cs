using UnityEngine;

public class PoisonedStatus : StatusEffect
{
    private int tickDamage;
    private float tickTimer = 0;
    private float damageInterval = 0.5f;

    public PoisonedStatus(int tickDamage, string effectText)
    {
        type = StatusType.Poison;
        this.tickDamage = tickDamage;
        this.effectText = effectText;
        this.effectColour = Color.darkGreen;
        isStackable = true;
    }  
    
    protected override void OnApplication()
    {
        base.OnApplication();
        tickTimer = 0;
    }

    protected override void OnUpdate()
    {
        tickTimer += Time.deltaTime;
        if ( tickTimer > damageInterval)
        {
            entityRef.OnTakeDamage(tickDamage, Color.darkGreen, DamageType.Poison);
            tickTimer = 0;
        }
    }

    protected override void OnRemoval()
    {    
        base.OnRemoval();
    }
    
}

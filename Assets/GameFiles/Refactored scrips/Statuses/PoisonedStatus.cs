using Unity.VisualScripting;
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

    protected override void OnFirstStackApplication()
    {
        entityRef.bodySystem.ApplyShader(effectColour * 3, 0.75f, ShaderType.Poisoned);
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

    protected override void OnLastStackRemoval()
    {
        entityRef.bodySystem.RemoveShader(0.5f, ShaderType.Poisoned);
    }

    public override StatusEffect Clone()
    {
        return new PoisonedStatus(tickDamage, effectText);
    }
}

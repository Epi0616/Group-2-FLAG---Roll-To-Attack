using UnityEngine;

public class ExampleNewEnemy : BaseAISlamEnemy
{
    public override void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        if (statusEffect.effect.type == StatusType.Knockback)
        {
            statusSystem.OnRecieveEffect(statusEffect);
        }
        else
        {
            OnRecieveHeal(15, Color.green);
            textDisplaySystem.DisplayHigherText("Absorbed", Color.green, 52);
        }
            
    }
    public override void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {
        if (statusEffect.effect.type == StatusType.Knockback)
        {
            statusSystem.OnRecieveEffect(statusEffect);
            textDisplaySystem.DisplayHigherText(statusEffect.effect.GetEffectText(), effectColour, 52);
        }
        else
        {
            OnRecieveHeal(15, Color.green);
            textDisplaySystem.DisplayHigherText("Absorbed", Color.green, 52);
        }
    }
}

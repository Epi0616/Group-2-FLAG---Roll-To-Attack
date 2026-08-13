using UnityEngine;

public class SlowStatus : StatusEffect
{
    private float slowMultiplier;
    private IMoveable moveInferfaceAccess;

    public SlowStatus(float slowMultiplier, string effectText)
    {
        type = StatusType.Slow;
        this.slowMultiplier = slowMultiplier;
        this.effectText = effectText;
        this.effectColour = Color.white;
        isStackable = true;
    }

    protected override void OnApplication()
    {
        moveInferfaceAccess = entityRef as IMoveable;
        isActive = moveInferfaceAccess != null;
        entityRef.bodySystem.ApplyShaderPowerIncrement(effectColour, 0.34f, 0.25f, ShaderType.Slow);
    }

    protected override void ApplyStatModifier()
    {               
        moveInferfaceAccess.movementSpeed.AddMultiplier(slowMultiplier);               
    }

    protected override void OnRemoval()
    {
        entityRef.bodySystem.RemoveShaderPowerIncrement(0.34f, 0.25f, ShaderType.Slow);
        base.OnRemoval();
    }

    public override StatusEffect Clone()
    {
        return new SlowStatus(slowMultiplier, effectText);
    }
}

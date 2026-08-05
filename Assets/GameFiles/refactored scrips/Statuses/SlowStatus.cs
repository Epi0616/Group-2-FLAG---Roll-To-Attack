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
        entityRef.bodySystem.ApplySlowShader();
    }

    protected override void ApplyStatModifier()
    {               
        moveInferfaceAccess.movementSpeed.AddMultiplier(slowMultiplier);               
    }

    protected override void OnRemoval()
    {
        entityRef.bodySystem.RemoveSlowShader();
        base.OnRemoval();
    }

    public override StatusEffect Clone()
    {
        return new SlowStatus(slowMultiplier, effectText);
    }
}

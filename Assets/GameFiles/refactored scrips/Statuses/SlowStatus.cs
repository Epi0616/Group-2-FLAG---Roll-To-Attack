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
    }

    protected override void ApplyStatModifier()
    {               
        moveInferfaceAccess.movementSpeed.AddMultiplier(slowMultiplier);               
    }
}

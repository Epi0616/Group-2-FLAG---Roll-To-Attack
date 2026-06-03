using UnityEngine;

public class SlowStatus : StatusEffect
{
    private float slowMultiplier;
    private IMoveable temp;

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
        isActive = entityRef is IMoveable temp;
    }

    protected override void ApplyStatModifier()
    {               
        temp.movementSpeed.AddMultiplier(slowMultiplier);               
    }
}

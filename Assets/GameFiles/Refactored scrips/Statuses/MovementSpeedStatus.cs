using UnityEngine;

public class MovementSpeedStatus : StatusEffect
{
    private IMoveable moveable;
    private float flatMult;

    public MovementSpeedStatus(float flatMult) 
    {
        this.flatMult = flatMult;
        type = StatusType.Speed;
    }
    protected override void OnApplication()
    {
        if (entityRef is IMoveable moveable)
        {
            this.moveable = moveable;

            isActive = true;
        }
    }

    protected override void ApplyStatModifier()
    {
        moveable.movementSpeed.AddMultiplierFlat(flatMult);
    }

}

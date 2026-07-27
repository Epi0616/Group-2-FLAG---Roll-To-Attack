using UnityEngine;

public class ConfusedStatus : StatusEffect
{
    private float confusedMultiplier;
    private IMoveable moveInferfaceAccess;

    public ConfusedStatus(float confusedMultiplier, string effectText)
    {
        type = StatusType.Confused;
        this.confusedMultiplier = confusedMultiplier;
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
        moveInferfaceAccess.movementSpeed.AddMultiplier(-confusedMultiplier);
    }
}

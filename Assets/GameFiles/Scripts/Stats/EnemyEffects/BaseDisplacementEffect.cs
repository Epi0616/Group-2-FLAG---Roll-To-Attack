using UnityEngine;

public class BaseDisplacementEffect : StatusEffect
{
    protected IKnockbackable knockbackInterfaceAccess;

    protected override void OnApplication()
    {
        isActive = entityRef is IKnockbackable knockbackInterfaceAccess;

        preventsMovement = true;
        preventsAction = true;
        isDisplacing = true;
        isStackable = true;
    }

    protected override void OnRemoval()
    {
        preventsMovement = false;
        preventsAction = false;
        isDisplacing = false;
        base.OnRemoval();
    }
}

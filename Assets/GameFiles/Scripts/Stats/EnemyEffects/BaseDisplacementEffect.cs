using UnityEngine;

public class BaseDisplacementEffect : StatusEffect
{
    protected IKnockbackable knockbackInterfaceAccess;

    protected override void OnApplication()
    {
        knockbackInterfaceAccess = entityRef as IKnockbackable;
        isActive = knockbackInterfaceAccess != null;

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

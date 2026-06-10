using UnityEngine;

public class BaseDisplacementEffect : StatusEffect
{
    protected IKnockbackable knockbackInterfaceAccess;
    protected IUsesRigidBody rbInterfaceAccess;

    protected override void OnApplication()
    {
        knockbackInterfaceAccess = entityRef as IKnockbackable;
        rbInterfaceAccess = entityRef as IUsesRigidBody;
        isActive = knockbackInterfaceAccess != null && rbInterfaceAccess != null;

        if (knockbackInterfaceAccess == null)
        {
            Debug.Log("KB missing");
        }
        if (rbInterfaceAccess == null)
        {
            Debug.Log("RB missing");
        }
        if (entityRef == null)
        {
            Debug.Log("Entity Missing");
        }

        if (!isActive) { toBeRemoved = true; return; }

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

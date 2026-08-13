using UnityEngine;

public class SlimeDisplacement : BaseDisplacementEffect
{
    protected override void OnApplication()
    {
        knockbackInterfaceAccess = entityRef as IKnockbackable;
        rbInterfaceAccess = entityRef as IUsesRigidBody;
        isActive = knockbackInterfaceAccess != null && rbInterfaceAccess != null;

        if (entityRef is INavAgent temp)
        {
            temp.DisableAIAgent();
        }

        //if (knockbackInterfaceAccess == null)
        //{
        //    Debug.Log("KB missing");
        //}
        //if (rbInterfaceAccess == null)
        //{
        //    Debug.Log("RB missing");
        //}
        //if (entityRef == null)
        //{
        //    Debug.Log("Entity Missing");
        //}

        if (!isActive) { toBeRemoved = true; return; }

        preventsMovement = true;
        isDisplacing = true;
        isStackable = true;
    }
}

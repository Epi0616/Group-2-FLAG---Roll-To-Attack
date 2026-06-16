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

        if (entityRef is INavAgent temp)
        {
            temp.DisableAIAgent();
        }

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

    protected override void OnFixedUpdate()
    {
        if (isActive && rbInterfaceAccess.rb.linearVelocity.y < 0)
        {
            
            rbInterfaceAccess.rb.AddForce(new Vector3(0, -2.0f, 0), ForceMode.Impulse);
            
        }
    }

    protected override void OnRemoval()
    {
        preventsMovement = false;
        preventsAction = false;
        isDisplacing = false;
        base.OnRemoval();
    }
}

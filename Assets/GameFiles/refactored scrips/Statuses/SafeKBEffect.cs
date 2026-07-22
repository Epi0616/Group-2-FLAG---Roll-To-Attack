using UnityEngine;

public class SafeKBEffect : StatusEffect
{
    Vector3 origin;
    float force;
    protected IKnockbackable knockbackInterfaceAccess;
    protected IUsesRigidBody rbInterfaceAccess;
    public SafeKBEffect(Vector3 origin, float force)
    {
        this.origin = origin;
        this.force = force;
        type = StatusType.Knockback;
        preventsMovement = true;
        preventsAction = true;
        isDisplacing = false;
        isStackable = true;
    }

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

        rbInterfaceAccess.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (entityRef.transform.position - origin);

        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0.3f;
        rbInterfaceAccess.rb.AddForce(targetDirection * ((force * knockbackInterfaceAccess.knockbackWeightMod.GetFinalValue()) * 10f), ForceMode.VelocityChange);
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

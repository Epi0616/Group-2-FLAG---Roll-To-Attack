using UnityEngine;

public class BaseStunEffect : StatusEffect
{
    protected IStunable stunInterfaceAccess;

    public BaseStunEffect()
    {
        type = StatusType.Stun;
    }

    protected override void OnApplication()
    {
        stunInterfaceAccess = entityRef as IStunable;
        isActive = stunInterfaceAccess != null;

        preventsMovement = true;
        preventsAction = true;       
    }

    protected override void OnRemoval()
    {
        preventsMovement = false;
        preventsAction = false;        
    }

    public override StatusEffect Clone()
    {
        return new BaseStunEffect();
    }
}

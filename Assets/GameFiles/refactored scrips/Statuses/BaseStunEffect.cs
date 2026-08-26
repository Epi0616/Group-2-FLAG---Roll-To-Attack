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
        if (stunInterfaceAccess == null) { toBeRemoved = true; return; }
        if (!stunInterfaceAccess.canBeStunned)
        {
            entityRef.textDisplaySystem.DisplayText("Resisted", effectColour, 64);
            toBeRemoved = true;
            return;
        }

        isActive = stunInterfaceAccess != null;

        preventsMovement = true;
        preventsAction = true;       

        stunInterfaceAccess.ResetStunInterval();
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

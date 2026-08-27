using UnityEngine;

public class BaseStunEffect : StatusEffect
{
    protected IStunable stunable;

    public BaseStunEffect()
    {
        type = StatusType.Stun;
    }

    protected override void OnApplication()
    {
        stunable = entityRef as IStunable;
        if (stunable == null) { toBeRemoved = true; return; }
        if (!stunable.canBeStunned)
        {
            entityRef.textDisplaySystem.DisplayText("Resisted", effectColour, 64);
            toBeRemoved = true;
            return;
        }

        isActive = stunable != null;

        preventsMovement = true;
        preventsAction = true;       

        stunable.ResetStunInterval();
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

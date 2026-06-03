using UnityEngine;

public class BaseStunEffect : StatusEffect
{

    public BaseStunEffect()
    {
        type = StatusType.Stun;
    }

    protected override void OnApplication()
    {
        isActive = entityRef is IStunable temp && temp.canBeStunned;

        preventsMovement = true;
        preventsAction = true;       
    }

    protected override void OnRemoval()
    {
        preventsMovement = false;
        preventsAction = false;        
    }
}

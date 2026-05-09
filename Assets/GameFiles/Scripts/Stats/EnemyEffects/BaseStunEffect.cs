using UnityEngine;

public class BaseStunEffect : StatusEffect
{

    public BaseStunEffect()
    {
        type = StatusType.Stun;
    }

    protected override void OnApplication()
    {
        preventsMovement = true;
        preventsAttack = true;
        disablesAI = true;
    }

    protected override void OnRemoval()
    {
        preventsMovement = false;
        preventsAttack = false;
        disablesAI = false;
    }
}

using UnityEngine;

public class BaseStunEffect : StatusEffect
{

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

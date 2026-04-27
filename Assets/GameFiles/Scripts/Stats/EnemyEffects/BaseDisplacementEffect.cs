using UnityEngine;

public class BaseDisplacementEffect : BaseStunEffect
{
    protected override void OnApplication()
    {
        base.OnApplication();
        isDisplacing = true;
        isStackable = true;
    }

    protected override void OnRemoval()
    {
        isDisplacing = false;
        base.OnRemoval();
    }
}

using UnityEngine;

public class PreventActionStatus : StatusEffect
{
    protected override void OnApplication()
    {
        preventsAction = true;
    }
}

using UnityEngine;

public class DragonStatusSystem : EntityStatusSystem
{
    public override bool CheckForActionBlockersStatus()
    {
        for (int i = currentActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            ActiveStatusEffect currentEffect = currentActiveStatusEffects[i];

            if (currentEffect.effect.type == StatusType.Knockback) continue;
            if (currentActiveStatusEffects[i].effect.preventsAction) return true;
        }
        return false;
    }
}

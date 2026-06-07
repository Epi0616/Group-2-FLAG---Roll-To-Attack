using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Localization;

[Serializable]
public class FreezeSlamAction : BaseSlamAction
{
    public float FreezeDuration = 1f;
    public float FragileDamageMult = 2f;
    public LocalizedString frozenText;


    public FreezeSlamAction() { }

    public override void ApplyCustomEffect(Entity hitEntity)
    {
        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new FreezeStatus(FragileDamageMult, "frozen" ),
                new List<BaseCondition> { new TimeCondition(true, hitEntity, FreezeDuration) }), slamColour);
    }
}
// frozenText.GetLocalizedString()
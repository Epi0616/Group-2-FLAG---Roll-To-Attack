using UnityEngine;
using System.Collections.Generic;

public class ActiveStatusEffect
{
    // Change this condition to a list of conditions


    public StatusEffect effect;
    public List<IEffectExpirationCondition> conditions;

    public ActiveStatusEffect(StatusEffect effect, List<IEffectExpirationCondition> conditionList)
    {
        this.effect = effect;
        conditions = conditionList;
    }

    public bool CheckForExpiration()
    {
        foreach (IEffectExpirationCondition condition in conditions)
        {

        }
    }

}

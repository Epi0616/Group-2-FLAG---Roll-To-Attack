using UnityEngine;
using System.Collections.Generic;

public class ActiveStatusEffect
{
    // Change this condition to a list of conditions


    public StatusEffect effect;
    public List<BaseCondition> conditions;
    public int numRequired = 0;

    public ActiveStatusEffect(StatusEffect effect, List<BaseCondition> conditionList)
    {
        this.effect = effect;
        conditions = conditionList;

        foreach (BaseCondition condition in conditions)
        {
            if (condition.isRequired)
            {
                numRequired++;  
            }
        }
    }

    public bool CheckForExpiration()
    {    
        bool allRequiredPresent = false;
        bool anyNonRequiredPresent = false;

        foreach (BaseCondition condition in conditions)
        {
            if (condition.isRequired)
            {
                allRequiredPresent = true;

                if (!condition.IsConditionMet())
                {
                    //Debug.Log("missing required condition");
                    return false;
                }
                
            }
            else
            {
                if (condition.IsConditionMet())
                {
                    //Debug.Log("optional condition present");
                    anyNonRequiredPresent = true;
                }
            }           
        }
        if (allRequiredPresent)
        {
            return true;
        }

        return anyNonRequiredPresent;
    }

    public void UpdateConditionsAll()
    {
        foreach (BaseCondition condition in conditions)
        {
            condition.ConditionUpdate();   
        }
    }

    public void ResetConditionsAll()
    {
        foreach (BaseCondition condition in conditions)
        {
            condition.ResetCondition();
        }
    }

}

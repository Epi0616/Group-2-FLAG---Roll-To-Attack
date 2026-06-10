using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ActionSelectionSystem
{
    private Entity entity;
    private IModifiableActions modifiableActions;
    public int LastReturnedActionIndex = 1;

    public ActionSelectionSystem(Entity entity)
    {
        this.entity = entity;
        modifiableActions = entity as IModifiableActions;
    }

    public ModifiableAction GetRandomModifiableAction()
    {
        List<ModifiableAction> weightedActions = modifiableActions.modifiableActions;
        int totalWeight = 0;
        LastReturnedActionIndex = 0;

        foreach (var action in weightedActions)
        {
            totalWeight += action.weighting;
        }
        int randomNumber = Random.Range(1, totalWeight + 1);
        int ActionWeightTally = 0;

        foreach (var action in weightedActions)
        {
            ActionWeightTally += action.weighting;
            if (randomNumber <= ActionWeightTally)
            { 
                return action;
            }
            LastReturnedActionIndex++;
        }

        Debug.LogError("NO VALID ACTIONS PRESENT BRO WTF HAPPEBNED");
        ActionWeightTally = 1;
        return null;
    }
}

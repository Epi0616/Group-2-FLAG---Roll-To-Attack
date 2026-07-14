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

    public void SetModifiableActions(List<ModifiableAction> newActions)
    {
        modifiableActions.playerLoadOut.WriteAbilities(newActions);
        modifiableActions.modifiableActions = newActions;
    }
    public void SetModifiableActionStorage(List<ModifiableAction> newActions)
    {
        modifiableActions.modifiableActionStorage = newActions;
    }

    public ConditionalAction GetRandomConditionalAction()
    {
        //List<EquippableActionHolder> weightedActions = ;
        int totalWeight = 0;
        LastReturnedActionIndex = 0;

        foreach (var action in modifiableActions.modifiableActions)
        {
            totalWeight += action.weighting;
        }
        int randomNumber = Random.Range(1, totalWeight + 1);
        int ActionWeightTally = 0;

        foreach (var action in modifiableActions.modifiableActions)
        {
            ActionWeightTally += action.weighting;
            if (randomNumber <= ActionWeightTally)
            { 
                return action.conditionalAction;
            }
            LastReturnedActionIndex++;
        }

        Debug.LogError("NO VALID ACTIONS PRESENT BRO WTF HAPPEBNED");
        ActionWeightTally = 1;
        return null;
    }
}

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
        UpdateAbilityDisplay();
    }
    public void SetModifiableActionStorage(List<ModifiableAction> newActions)
    {
        modifiableActions.modifiableActionStorage = newActions;
    }

    public void UpdateAbilityDisplay()
    {
        List<ModifiableAction> newActions = modifiableActions.playerLoadOut.ReadAbilities();
        for (int i = 0; i < newActions.Count; i++)
        {
            //Debug.Log("New Name is: " +  newActions[i].actionName.GetLocalizedString() + " at index: " + i);
            modifiableActions.displaySlots[i].sprite = newActions[i].sprite;
        }
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

    public ModifiableAction GetRandomModifiableAction()
    {
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
                //Debug.Log("Selected: " + action.actionName.GetLocalizedString());
                return action;
            }
            LastReturnedActionIndex++;
        }

        Debug.LogError("NO VALID ACTIONS PRESENT BRO WTF HAPPEBNED");
        ActionWeightTally = 1;
        return null;
    }
}

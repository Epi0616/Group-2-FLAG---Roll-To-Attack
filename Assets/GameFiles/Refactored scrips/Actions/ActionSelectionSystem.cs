using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public void SetIndexedModifiableActions(List<IndexedModifiableAction> newIndexedActions)
    {
        modifiableActions.playerLoadOut.WriteAbilities(newIndexedActions);

        modifiableActions.indexedModifiableActions = newIndexedActions;
        UpdateAbilityDisplay();
    }
    public void SetModifiableActionStorage(List<ModifiableAction> newActions)
    {
        modifiableActions.modifiableActionStorage = newActions;
    }

    public void UpdateAbilityDisplay()
    {
        List<IndexedModifiableAction> indexedModifiableActions = modifiableActions.playerLoadOut.ReadAbilities();

        for (int i = 0; i < modifiableActions.displaySlots.Length; i++)
        {
            for (int j = 0; j < indexedModifiableActions.Count; j++)
            {
                if (indexedModifiableActions[j].index == i)
                {
                    modifiableActions.displaySlots[i].sprite = indexedModifiableActions[j].modifiableAction.sprite;
                    //Debug.Log("New Name is: " +  newActions[i].actionName.GetLocalizedString() + " at index: " + i);
                    break;
                }
                modifiableActions.displaySlots[i].sprite = null;
            }
        }
    }

    //update with modifiable action logic/for loop
    public ConditionalAction GetRandomConditionalAction()
    {
        //List<EquippableActionHolder> weightedActions = ;
        int totalWeight = 0;
        LastReturnedActionIndex = 0;

        List<IndexedModifiableAction> indexedModifiableActions = modifiableActions.indexedModifiableActions;
        for (int i = 0; i < modifiableActions.maxActions; i++)
        {
            if (indexedModifiableActions[i] != null)
            { 
                ModifiableAction modifiableAction = indexedModifiableActions[i].modifiableAction;
                totalWeight += modifiableAction.weighting;
                continue;
            }

            totalWeight += modifiableActions.baseAction.weighting;
        }

        int randomNumber = Random.Range(1, totalWeight + 1);
        int ActionWeightTally = 0;

        for (int i = 0; i < modifiableActions.maxActions; i++)
        {
            ModifiableAction modifiableAction = modifiableActions.baseAction;

            if (indexedModifiableActions[i] != null)
            {
                modifiableAction = indexedModifiableActions[i].modifiableAction;
            }

            totalWeight += modifiableAction.weighting;
            if (randomNumber <= ActionWeightTally)
            {
                return modifiableAction.conditionalAction;
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

        List<IndexedModifiableAction> indexedModifiableActions = modifiableActions.indexedModifiableActions;
        for (int i = 0; i < modifiableActions.maxActions; i++)
        {
            ModifiableAction modifiableAction = modifiableActions.baseAction;
            for (int j = 0; j < indexedModifiableActions.Count; j++)
            {
                if (indexedModifiableActions[j].index == i)
                { 
                    modifiableAction = indexedModifiableActions[j].modifiableAction;
                }
            }

            totalWeight += modifiableAction.weighting;
        }

        int randomNumber = Random.Range(1, totalWeight + 1);
        int actionWeightTally = 0;

        for (int i = 0; i < modifiableActions.maxActions; i++)
        {
            ModifiableAction modifiableAction = modifiableActions.baseAction;

            for (int j = 0; j < indexedModifiableActions.Count; j++)
            {
                if (indexedModifiableActions[j].index == i)
                {
                    modifiableAction = indexedModifiableActions[j].modifiableAction;
                }
            }

            actionWeightTally += modifiableAction.weighting;
            if (randomNumber <= actionWeightTally)
            {
                return modifiableAction;
            }
            LastReturnedActionIndex++;
        }

        Debug.LogError("NO VALID ACTIONS PRESENT BRO WTF HAPPEBNED");
        actionWeightTally = 1;
        return null;
    }
}

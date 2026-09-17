using UnityEngine;
using System;
using System.Collections.Generic;

public class SetUpArenaManager : MonoBehaviour
{
    public static event Action<int, int, List<IndexedModifiableAction>, List<IndexedModifiableAction>> setUpPlayer;
    public static event Action<int, bool> SetUpWavePosition;

    private void OnEnable()
    {
        SceneTransitionManager.TransitionComplete += SetUpGame;
    }

    private void OnDisable()
    {
        SceneTransitionManager.TransitionComplete -= SetUpGame;
    }

    private void SetUpGame()
    {
        GameSaveData currentSaveData = GameManager.instance.gameSaveData;

        List<IndexedModifiableAction> playerAbilities = ConvertAbilityDataToIndexedActions(currentSaveData.playerAbilityData);
        List<IndexedModifiableAction> playerStorage = ConvertAbilityDataToIndexedActions(currentSaveData.playerStorageData);
        setUpPlayer?.Invoke(currentSaveData.currentHp, currentSaveData.maxHp, playerAbilities, playerStorage);
        SetUpWavePosition?.Invoke(currentSaveData.waveNumber, currentSaveData.waveCleared);

        RunTimeStatTracker.instance.runTimeStats = currentSaveData.runTimeStats;
    }

    private List<IndexedModifiableAction> ConvertAbilityDataToIndexedActions(List<AbilityData> abilityData)
    {
        List<IndexedModifiableAction> indexedModifiableActions = new List<IndexedModifiableAction>();

        foreach (AbilityData ability in abilityData)
        {
            ModifiableActionDescriptor modifiableActionDescriptor = AbilityLookUpTable.instance.GetAbilityObjFromType(ability.type);
            ModifiableAction modifiableAction = SetUpModifiableAction(modifiableActionDescriptor.Create(), ability.level);
            IndexedModifiableAction temp = new IndexedModifiableAction(ability.index, modifiableAction);
            indexedModifiableActions.Add(temp);
        }

        return indexedModifiableActions;
    }

    private ModifiableAction SetUpModifiableAction(ModifiableAction modifiableAction, int level)
    {
        if (modifiableAction.conditionalAction.action is not IUpgradableAbility upgradableAbility) return modifiableAction;

        if (level <= 0) return modifiableAction;

        ModifiableAction upgradedAbility = upgradableAbility.upgradeResult.Create();
        upgradedAbility.UpdateEnhancementLevel(level);

        return upgradedAbility;
    }
}

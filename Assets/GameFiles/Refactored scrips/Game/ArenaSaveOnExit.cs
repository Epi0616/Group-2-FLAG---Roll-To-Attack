using System.Collections.Generic;
using UnityEngine;

public class ArenaSaveOnExit : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Player player;

    private void OnEnable()
    {
        PauseMenu.PackUpScene += SaveGame;
    }

    private void OnDisable()
    {
        PauseMenu.PackUpScene -= SaveGame;
    }

    private void SaveGame()
    {
        SaveGameData();
    }

    private void SaveGameData()
    {
        if (player == null) return;

        int currentHp = player.healthSystem.currentHealth;
        int maxHp = (int)player.healthSystem.maxHealth.GetFinalValue();
        int waveNumber = waveManager.currentWaveIndex;
        bool waveCleared = waveManager.waveCleared;
        List<AbilityData> playerAbilities = ConvertActionsToAbilityData(player.playerLoadOut.ReadAbilities());
        RunTimeStats runTimeStats = RunTimeStatTracker.instance.runTimeStats;
        int seed = GameManager.instance.gameSaveData.seed;

        GameSaveData currentSaveData = new GameSaveData(currentHp, maxHp, waveNumber, waveCleared, playerAbilities, runTimeStats, seed);
        SaveSystem.instance.SaveGameData(currentSaveData);
        GameManager.instance.gameSaveData = currentSaveData;
    }

    private List<AbilityData> ConvertActionsToAbilityData(List<IndexedModifiableAction> indexedModifiableActions)
    { 
        List<AbilityData> returnList = new List<AbilityData>();
        foreach (var indexedAction in indexedModifiableActions)
        { 
            int index = indexedAction.index;
            int level = indexedAction.modifiableAction.enhancementLevel;
            AbilityType type = indexedAction.modifiableAction.abilityType;

            returnList.Add(new AbilityData(index, level, type));
        }

        return returnList;
    }
}

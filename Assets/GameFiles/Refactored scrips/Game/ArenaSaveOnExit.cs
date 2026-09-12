using System.Collections.Generic;
using UnityEngine;

public class ArenaSaveOnExit : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    private Player player;

    private void OnEnable()
    {
        PauseMenu.PackUpScene += SaveGame;
    }

    private void OnDisable()
    {
        PauseMenu.PackUpScene -= SaveGame;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void SaveGame()
    {
        SaveGameData();
    }

    private void SaveGameData()
    {
        int currentHp = player.healthSystem.currentHealth;
        int maxHp = (int)player.healthSystem.maxHealth.GetFinalValue();
        int waveNumber = waveManager.currentWaveIndex;
        bool waveCleared = waveManager.waveCleared;
        List<IndexedModifiableAction> indexedModifiableActions = player.playerLoadOut.ReadAbilities();
        RunTimeStats runTimeStats = RunTimeStatTracker.instance.runTimeStats;
        int seed = GameManager.instance.gameSaveData.seed;

        GameSaveData currentSaveData = new GameSaveData(currentHp, maxHp, waveNumber, waveCleared, indexedModifiableActions, runTimeStats, seed);
        SaveSystem.instance.SaveGameData(currentSaveData);
        GameManager.instance.gameSaveData = currentSaveData;
    }
}

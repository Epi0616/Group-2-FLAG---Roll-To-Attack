using UnityEngine;
using System;
using System.Collections.Generic;

public class SetUpArenaManager : MonoBehaviour
{
    public static event Action<int, int, List<IndexedModifiableAction>> setUpPlayer;
    public static event Action<int, bool> SetUpWavePosition;

    private void Start()
    {
        GameSaveData currentSaveData = GameManager.instance.gameSaveData;

        setUpPlayer?.Invoke(currentSaveData.currentHp, currentSaveData.maxHp, currentSaveData.playerAbilities);
        SetUpWavePosition?.Invoke(currentSaveData.waveNumber, currentSaveData.waveCleared);

        RunTimeStatTracker.instance.runTimeStats = currentSaveData.runTimeStats;
    }
}

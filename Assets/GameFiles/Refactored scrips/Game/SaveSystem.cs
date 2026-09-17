using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private string path;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        path = Path.Combine(Application.persistentDataPath, "saveData.json");
        CheckForValidSaveFile();
    }

    private void CheckForValidSaveFile()
    {
        if (!File.Exists(path))
        {
            CreateNewSave();
        }
    }

    public void SaveGameData(GameSaveData saveData)
    { 
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }

    public GameSaveData LoadSaveGameData()
    {
        if (!File.Exists(path))
        {
            return CreateNewSave();
        }

        string json = File.ReadAllText(path);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
        return saveData;
    }

    private GameSaveData CreateNewSave()
    {
        int seed = (int)DateTime.Now.Ticks;
        GameSaveData newSave = new GameSaveData(100, 100, 1, false, new List<AbilityData>(), new List<AbilityData>(), new RunTimeStats(), seed);
        SaveGameData(newSave);

        return newSave;
    }
}

[Serializable]
public class GameSaveData
{
    public int currentHp, maxHp;
    public int waveNumber;
    public bool waveCleared;

    public List<AbilityData> playerAbilityData;
    public List<AbilityData> playerStorageData;
    public RunTimeStats runTimeStats;

    public int seed;

    public GameSaveData(int currentHp, int maxHp, int waveNumber, bool waveCleared, List<AbilityData> playerAbilityData, List<AbilityData> playerStorageData, RunTimeStats runTimeStats, int seed)
    {
        this.currentHp = currentHp;
        this.maxHp = maxHp;
        this.waveNumber = waveNumber;
        this.waveCleared = waveCleared;
        this.playerAbilityData = playerAbilityData;
        this.playerStorageData = playerStorageData;
        this.runTimeStats = runTimeStats;
        this.seed = seed;
    }
}

[Serializable]
public class AbilityData
{
    public int index;
    public int level;
    public AbilityType type;

    public AbilityData(int index, int level, AbilityType type)
    { 
        this.index = index;
        this.level = level;
        this.type = type;
    }
}
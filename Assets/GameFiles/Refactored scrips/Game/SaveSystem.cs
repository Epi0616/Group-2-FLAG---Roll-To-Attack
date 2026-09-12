using System.Collections.Generic;
using System.IO;
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
        Debug.Log(path);
    }

    public void SaveGameData(GameSaveData saveData)
    { 
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }

    public GameSaveData LoadSaveGameData()
    { 
        string json = File.ReadAllText(path);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
        return saveData;
    }
}

public class GameSaveData
{
    public int currentHp, maxHp;
    public int waveNumber;
    public bool waveCleared;

    public List<IndexedModifiableAction> playerAbilities;
    public RunTimeStats runTimeStats;

    public int seed;

    public GameSaveData(int currentHp, int maxHp, int waveNumber, bool waveCleared, List<IndexedModifiableAction> playerAbilities, RunTimeStats runTimeStats, int seed)
    {
        this.currentHp = currentHp;
        this.maxHp = maxHp;
        this.waveNumber = waveNumber;
        this.waveCleared = waveCleared;
        this.playerAbilities = playerAbilities;
        this.runTimeStats = runTimeStats;
        this.seed = seed;
    }
}

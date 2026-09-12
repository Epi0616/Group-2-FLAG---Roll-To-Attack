using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameSaveData gameSaveData { get; set; }

    public void OnEnable()
    {
        IntroSceneMenuUI.newGame += StartNewGame;
        IntroSceneMenuUI.loadGame += LoadGame;
    }

    public void OnDisable()
    {
        IntroSceneMenuUI.newGame -= StartNewGame;
        IntroSceneMenuUI.loadGame -= LoadGame;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartNewGame()
    {
        int seed = (int)DateTime.Now.Ticks;
        gameSaveData = new GameSaveData(100, 100, 1, false, new List<IndexedModifiableAction>(), new RunTimeStats(), seed);
    }

    public void LoadGame()
    { 
        gameSaveData = SaveSystem.instance.LoadSaveGameData();
    }
}
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ArenaLoadManager : MonoBehaviour, IInitializeable
{
    [SerializeField] private List<GameObject> arenaPrefabs = new List<GameObject>();

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (GameObject obj in arenaPrefabs)
        {
            Instantiate(obj, transform);
        }
    }
}

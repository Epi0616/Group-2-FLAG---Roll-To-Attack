using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LoadManager : MonoBehaviour, IInitializeable
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (GameObject obj in prefabs)
        {
            Instantiate(obj, transform);
        }
    }
}

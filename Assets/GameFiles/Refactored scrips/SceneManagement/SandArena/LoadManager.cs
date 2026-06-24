using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class LoadManager : MonoBehaviour, IInitializeable
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    public void Initialize()
    {
        foreach (GameObject obj in prefabs)
        {
            Instantiate(obj, transform);
        }
    }

    public IEnumerator InitializeAsync()
    {
        foreach (GameObject obj in prefabs)
        {
            AsyncInstantiateOperation instantiate = InstantiateAsync(obj);
            yield return instantiate;

            foreach (GameObject prefab in instantiate.Result)
            {
                prefab.transform.SetParent(transform, false);
                prefab.transform.localPosition = obj.transform.position;
                prefab.transform.localRotation = obj.transform.rotation;
                prefab.transform.localScale = obj.transform.localScale;
            }          
        }
    }
}

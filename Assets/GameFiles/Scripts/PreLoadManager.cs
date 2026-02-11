using UnityEngine;

public class PreLoadManager : MonoBehaviour
{
    [Header("Objects to be 'warmed up' before play")]
    public GameObject[] gameObjects;
    void Start()
    {
        foreach (var obj in gameObjects)
        { 
            GameObject tempObj = Instantiate(obj);
            Destroy(tempObj);
        }
    }
}

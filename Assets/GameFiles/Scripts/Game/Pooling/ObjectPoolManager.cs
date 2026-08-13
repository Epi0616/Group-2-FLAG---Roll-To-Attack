using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    public GameObject emptyHolder;

    public static GameObject gameObjectsEmpty;

    public static Dictionary<GameObject, ObjectPool<GameObject>> objectPools;
    public static Dictionary<GameObject, GameObject> cloneToPrefabMap;

    public static HashSet<GameObject> activeObjects { get; private set; }

    public enum PoolType
    {
        GameObjects = 0
    }
    public static PoolType poolingType;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
        activeObjects = new HashSet<GameObject>();

        SetUpEmpties();
    }

    private void SetUpEmpties()
    {
        emptyHolder = new GameObject("Object Pools");

        gameObjectsEmpty = new GameObject("Game Objects");
        gameObjectsEmpty.transform.SetParent(emptyHolder.transform);
    }

    private void OnDestroy()
    {
        ClearPools();
    }

    private void ClearPools()
    {
        if (objectPools != null)
        {
            foreach (var pool in objectPools.Values)
            {
                pool.Clear();
            }
            objectPools.Clear();
        }

        cloneToPrefabMap?.Clear();
        activeObjects?.Clear();
    }

    private static void CreatePool(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects)
    { 
        ObjectPool<GameObject> objectPool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, position, rotation, poolType), 
            actionOnGet : OnGetObject, 
            actionOnRelease : OnReleaseObject, 
            actionOnDestroy : OnDestroyObject
            );

        objectPools.Add(prefab, objectPool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects)
    {
        //prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, position, rotation);

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    private static void OnGetObject(GameObject obj)
    { 
        //optional logic
    }

    private static void OnReleaseObject(GameObject obj)
    { 
        if (obj != null)
        {
            activeObjects.Remove(obj);
            obj.SetActive(false);
        }
    }

    private static void OnDestroyObject(GameObject obj)
    { 
        if (cloneToPrefabMap.ContainsKey(obj))
        {
            cloneToPrefabMap.Remove(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObjects:
                return gameObjectsEmpty;

            default:
                return null;
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPosition, spawnRotation, poolType);
        }

        GameObject obj = objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!cloneToPrefabMap.ContainsKey(obj))
            {
                cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            //Debug.Log($"Pools: {objectPools.Count}, Active objects tracked: {cloneToPrefabMap.Count}");
            activeObjects.Add(obj);

            obj.transform.position = spawnPosition;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            { 
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"object {objectToSpawn.name} does not have component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
    { 
        return SpawnObject<T>(typePrefab.gameObject, spawnPosition, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    { 
        return SpawnObject<GameObject>(objectToSpawn, spawnPosition, spawnRotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if (cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);

            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                if (obj != null && prefab != null && pool != null)
                {
                    pool.Release(obj);
                    //Debug.Log($"Pools: {objectPools.Count}, Active objects tracked: {cloneToPrefabMap.Count}");
                    //Debug.Log(obj.ToString());
                }
                else
                {
                    Debug.LogWarning("null object attempted to release");
                }
                
            }
        }

        else 
        { 
            Debug.LogWarning("trying to return an object that is not pooled" + obj.name);
        }
    }

    public static void ReturnObjectToPool(GameObject obj, float spawnDelay, PoolType poolType = PoolType.GameObjects)
    {
        instance.StartCoroutine(ReturnAfterDelay(obj, spawnDelay, poolType));
    }
    private static IEnumerator ReturnAfterDelay(GameObject obj, float spawnDelay, PoolType poolType)
    {
        yield return new WaitForSeconds(spawnDelay);

        ReturnObjectToPool(obj, poolType);
    }
}



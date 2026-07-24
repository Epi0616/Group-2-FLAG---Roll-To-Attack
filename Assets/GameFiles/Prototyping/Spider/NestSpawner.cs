using System.Threading;
using UnityEngine;

public class NestSpawner : MonoBehaviour
{
    public GameObject creaturePrefab;
    public GameObject nest;
    Vector3 spawnPos;
    public float timer = 1;

    private void Start()
    {
        spawnPos = new Vector3(nest.transform.position.x, nest.transform.position.y, nest.transform.position.z - 5);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0 )
        {
            ObjectPoolManager.SpawnObject(creaturePrefab, spawnPos, nest.transform.rotation);
            timer = Random.Range(5, 10);
        }
    }
}

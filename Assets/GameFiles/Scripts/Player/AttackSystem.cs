using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackSystem : MonoBehaviour
{
    public GameObject impactField; //on field impact field
    public GameObject poisonImpactField, playerSpike, playerRocket, playerVacuum; //to be instantiated
    private List<GameObject> objectsInOrbit = new List<GameObject>();

    public GameObject InstantiateObejct(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);

        return newObject;
    }

    public void CreateRockets(EnemyStateController target)
    {
        GameObject rocket = Instantiate(playerRocket, transform.position, Quaternion.identity);
        rocket.GetComponent<PlayerRocket>().SetTarget(target, transform.position.y);

    }

    public void CreateVaccum(float range, float timer)
    {
        GameObject vacuum = Instantiate(playerVacuum, transform.position, Quaternion.identity);
        vacuum.GetComponent<PlayerVacuum>().SetUp(range, timer);
    }

    public void CreateFourPipSpikesInOrbit()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject spike = Instantiate(playerSpike);
            objectsInOrbit.Add(spike);
        }

        for (int i = 0; i < objectsInOrbit.Count; i++)
        {
            float angle = i * (360f / objectsInOrbit.Count);
            PlayerSpikeFixedYMod tempScript = objectsInOrbit[i].gameObject.GetComponent<PlayerSpikeFixedYMod>(); //will need to change between PlayerSpikeFixedYMod and PlayerSpike depending on desired functionality/script attatched to the spike prefab
            tempScript.Initialize(angle, gameObject);
        }
    }

    public void RemoveObjectFromOrbit(GameObject obj)
    {
        objectsInOrbit.Remove(obj);
    }

}

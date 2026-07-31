
using UnityEngine;

public class EggThrown : MonoBehaviour
{
    Rigidbody rb;
    public float timer = 3;
    public GameObject eggCreature;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(Vector3.forward * 200, ForceMode.Impulse);
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
            ObjectPoolManager.SpawnObject(eggCreature, transform.position, transform.rotation);
        }
    }
}
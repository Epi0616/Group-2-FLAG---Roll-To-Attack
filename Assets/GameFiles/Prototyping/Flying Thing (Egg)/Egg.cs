using UnityEngine;

public class Egg : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject creature;
    public GameObject egg;
    public EnemyHealthSystem enemyHealthSystem;

    void FixedUpdate()
    {
        egg.transform.Rotate(rotationSpeed * Time.fixedDeltaTime, 0, 0);

        if(enemyHealthSystem.isDead)
        {
            ObjectPoolManager.SpawnObject(creature, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

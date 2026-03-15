using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    [SerializeField] GameObject impactFieldPrefab;
    private EnemyStateController target;
    private bool searchingForTarget = false;
    private bool flyingTowardsTarget = false;
    private bool targetAssigned = false;
    private float startHeight;

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.up);
    }

    void Update()
    {
        if (target == null)
        {
            if (targetAssigned)
            {
                DestroyMe();
            }
            return;
        }
            


        if (!searchingForTarget)
        {
            FlyUp();
            return;
        }

        if (!flyingTowardsTarget)
        {
            SearchForTarget();
            return;
        }

        FlyTowardsTarget();
        
    }

    public void SetTarget(EnemyStateController target, float startHeight)
    { 
        this.target = target;
        this.startHeight = startHeight;
        transform.rotation = Quaternion.LookRotation(Vector3.up);
        targetAssigned = true;
        searchingForTarget = false;
        flyingTowardsTarget = false;
    }

    private void SearchForTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7.5f * Time.deltaTime);

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        if (angle < 5f)
        {
            flyingTowardsTarget = true;
        }
    }

    private void FlyTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = targetRotation;

        transform.position += transform.forward * 100f * Time.deltaTime;
    }

    private void FlyUp()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, startHeight + 30, target.transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = targetRotation;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 2f * Time.deltaTime);
        //transform.position += transform.forward * 65f * Time.deltaTime;
        if (transform.position.y >= startHeight + 25)
        { 
            searchingForTarget = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject Enemy = other.gameObject;
        if (Enemy.CompareTag("Enemy"))
        {
            DamageEnemy(Enemy);
        }
    }

    private void DamageEnemy(GameObject Enemy)
    {
        Vector3 groundedPosition = new(transform.position.x, 1.5f, transform.position.z); // needs adjusting if enemies can ever reach an elevated position.

        //Instantiate(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(1f, 1f, 0.5f, 1f);
        ObjectPoolManager.SpawnObject(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(1f, 1f, 0.5f, 1f);

        Enemy.GetComponent<EnemyStateController>().OnTakeDamage(40, Color.orange);
        DestroyMe();
    }

    private void DestroyMe()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

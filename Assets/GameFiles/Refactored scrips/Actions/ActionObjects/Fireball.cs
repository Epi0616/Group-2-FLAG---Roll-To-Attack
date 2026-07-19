using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    [SerializeField] protected GameObject impactFieldPrefab;
    [SerializeField] protected Vector3 startScale;

    protected Vector3 direction;
    private int impactDamage;
    private int fieldDamage;
    protected Entity ownerEntity;

    private bool hitTarget = false;
    public bool active = false;

    private void OnEnable()
    {
        transform.localScale = startScale;
    }

    public virtual void Initialize(Entity ownerEntity, Vector3 direction, int impactDamage, int fieldDamage)
    {
        this.ownerEntity = ownerEntity;
        this.direction = direction;
        this.impactDamage = impactDamage;
        this.fieldDamage = fieldDamage;

        active = true;
        hitTarget = false;
        StopAllCoroutines();
        StartCoroutine(Attack());
    }

    public void HandlePathToTarget()
    { 
        
    }

    private IEnumerator Attack()
    {
        while (!hitTarget)
        {
            FlyToTarget();
            yield return null;
        }
    }

    private void FlyToTarget()
    {
        transform.forward = direction;
        transform.position += transform.forward * 50f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!active) return;
        if (hitTarget) return;
        if (!(ownerEntity is IFireballAction fireballAction)) return;

        if ((fireballAction.targetableLayers.value & (1 << hit.gameObject.layer)) != 0)
        {
            hitTarget = true;
            OnHit();
        }
    }

    private void OnHit()
    {
        GameObject field =  ObjectPoolManager.SpawnObject(impactFieldPrefab, transform.position, Quaternion.identity);
        field.GetComponent<PoisonField>().Initialize(ownerEntity, 5f, 10f, fieldDamage, Color.orange);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor.Rendering;

public class Fireball : MonoBehaviour
{
    [SerializeField] protected GameObject impactFieldPrefab;

    private int initialDamage;
    private int tickDamage;
    protected Entity ownerEntity;
    private float speed;

    private Coroutine attackRoutine, lifeTimeRoutine;
    private bool hitTarget;

    public virtual void Initialize(Entity ownerEntity, float speed, int initialDamage, int tickDamage)
    {
        this.ownerEntity = ownerEntity;
        this.speed = speed;
        this.initialDamage = initialDamage;
        this.tickDamage = tickDamage;

        hitTarget = false;
        lifeTimeRoutine = StartCoroutine(lifeTime(10));
        attackRoutine = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (!hitTarget)
        {
            FlyToTarget();
            yield return null;
        }

        attackRoutine = null;
        OnHit();
    }

    private IEnumerator lifeTime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        lifeTimeRoutine = null;
        Interrupt();
    }

    private void FlyToTarget()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hitTarget) return;
        if (!(ownerEntity is IFireballAction fireballAction)) return;

        if ((fireballAction.targetableLayers.value & (1 << hit.gameObject.layer)) != 0)
        {
            hitTarget = true;
        }
    }

    private void OnHit()
    {
        GameObject field =  ObjectPoolManager.SpawnObject(impactFieldPrefab, transform.position, Quaternion.identity);
        field.GetComponent<FireField>().Initialize(ownerEntity, Color.orange, 5f, initialDamage, tickDamage, 10f, 1f);

        Interrupt();
    }

    public void Interrupt()
    {
        if (attackRoutine != null)
        { 
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        if (lifeTimeRoutine != null)
        {
            StopCoroutine(lifeTimeRoutine);
            lifeTimeRoutine = null;
        }

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

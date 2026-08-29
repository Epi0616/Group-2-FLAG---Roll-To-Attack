using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour, IDecalShadowCast
{
    [SerializeField] protected GameObject impactFieldPrefab;

    private int initialDamage;
    private int tickDamage;
    protected Entity ownerEntity;
    private float speed;

    private Coroutine attackRoutine, lifeTimeRoutine;
    private bool hitTarget;
    [SerializeField] private LayerMask groundLayer;

    public ShadowDecal currentShadowDecal { get; set; }
    [SerializeField] private GameObject ShadowDecalPrefab;
    public GameObject shadowDecalPrefab { get => ShadowDecalPrefab; set => ShadowDecalPrefab = value; }

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
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 50, groundLayer);
        Vector3 fieldPos = hit.point;
        fieldPos.y += Random.Range(-0.2f, 0.2f);
        GameObject field =  ObjectPoolManager.SpawnObject(impactFieldPrefab, fieldPos, Quaternion.identity);
        field.GetComponent<FireField>().Initialize(ownerEntity, Color.orange, 3.5f, initialDamage, tickDamage, 10f, 1f);

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
        if (currentShadowDecal != null)
        {
            currentShadowDecal.DestroyMe();
            currentShadowDecal = null;
        }
        
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

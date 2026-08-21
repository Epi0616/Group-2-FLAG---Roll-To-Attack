using System.Collections.Generic;
using UnityEngine;

public class RadialProjectile : MonoBehaviour
{
    private Vector3 startPos;
    private Entity ownerEntity;
    private float distance;
    private float speed = 0;

    private IRadialProjectile radialProjectile;
    private bool active = false;

    public void Initialize(Entity ownerEntity, float distance, float speed)
    {
        this.ownerEntity = ownerEntity;
        this.distance = distance;
        this.speed = speed;

        if (!(ownerEntity is IRadialProjectile radialProjectile)) { Debug.LogError("owner entity is not of type IRadialProjectile"); return; }
        this.radialProjectile = radialProjectile;

        startPos = transform.position;
        active = true;
    }

    private void Update()
    {
        if (!active) return;

        FlyToTarget();
        CheckForDistanceComplete();
    }

    private void FlyToTarget()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!active) return;

        if ((radialProjectile.radialTargetableLayers.value & (1 << hit.gameObject.layer)) != 0)
        {
            OnHit(hit.gameObject);
        }
    }

    private void OnHit(GameObject hit)
    {
        if (!hit.TryGetComponent<Entity>(out Entity entity)) return;

        entity.OnRecieveEffect(new ActiveStatusEffect(new KnockbackEffect(transform.position, 5f), new List<BaseCondition>() { new AlwaysTrueCondition() }, true));
        entity.OnTakeDamage(4, Color.red, DamageType.Spell);
    }

    private void CheckForDistanceComplete()
    {
        if ((transform.position - startPos).magnitude > distance)
        {
            OnDistanceReached();
        }
    }

    private void OnDistanceReached()
    {
        radialProjectile = null;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

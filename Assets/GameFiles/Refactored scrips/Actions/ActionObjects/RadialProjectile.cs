using System.Collections.Generic;
using UnityEngine;

public class RadialProjectile : MonoBehaviour
{
    private Vector3 startPos;
    private Entity ownerEntity;
    private float distance;
    private float speed = 0;

    private bool active = false;

    public void Initialize(Entity ownerEntity, float distance, float speed)
    {
        this.ownerEntity = ownerEntity;
        this.distance = distance;
        this.speed = speed;

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
        if (!(ownerEntity is IFireballAction fireballAction)) return;

        if ((fireballAction.targetableLayers.value & (1 << hit.gameObject.layer)) != 0)
        {
            OnHit(hit.gameObject);
        }
    }

    private void OnHit(GameObject hit)
    {
        if (!hit.TryGetComponent<Entity>(out Entity entity)) return;

        entity.OnRecieveEffect(new ActiveStatusEffect(new KnockbackEffect(transform.position, 5f), new List<BaseCondition>() { new AlwaysTrueCondition() }, true));
        entity.OnTakeDamage(10, Color.red, DamageType.Normal);
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
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

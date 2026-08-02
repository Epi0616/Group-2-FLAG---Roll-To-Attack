using UnityEngine;
using System.Collections;
using UnityEditor.Rendering;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class Fireball : MonoBehaviour
{
    [SerializeField] protected GameObject impactFieldPrefab;
    [SerializeField] protected Vector3 startScale;

    protected Vector3 direction;

    private int impactDamage;
    private int fieldDamage;
    private float radius;
    private float duration;
    private float speed;
    protected Entity ownerEntity;

    private bool hitTarget = false;
    public bool active = false;

    private void OnEnable()
    {
        active = false;
        hitTarget = false;
        transform.localScale = startScale;
    }

    private void OnDisable()
    {
        active = false;
        hitTarget = true;
        ownerEntity = null;

        StopAllCoroutines();
    }

    public virtual void Initialize(Entity ownerEntity, Vector3 direction, int impactDamage, int fieldDamage, float radius, float duration, float speed)
    {
        Debug.Log("initializing fireball");
        this.ownerEntity = ownerEntity;
        this.direction = direction;
        this.impactDamage = impactDamage;
        this.fieldDamage = fieldDamage;
        this.radius = radius;
        this.duration = duration;
        this.speed = speed;

        active = true;
        hitTarget = false;
        StopAllCoroutines();
        StartCoroutine(FlyToTarget());
    }

    private IEnumerator FlyToTarget()
    {
        while (!hitTarget)
        {
            transform.forward = direction;
            transform.position += transform.forward * 50f * Time.deltaTime;

            yield return null;
        }
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
        active = false;
        StopAllCoroutines();

        GameObject field =  ObjectPoolManager.SpawnObject(impactFieldPrefab, transform.position, Quaternion.identity);
        field.GetComponent<PoisonField>().Initialize(ownerEntity, radius, duration, fieldDamage, Color.orange);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void TryToCancel(Entity potentialOwner)
    {
        Debug.Log("trying to cancel fb");
        Debug.Log($"ownerEntity {ownerEntity}");
        Debug.Log($"potential owner {potentialOwner}");
        if (!active || potentialOwner != ownerEntity) return;

        Debug.Log("cancelling");

        active = false;
        hitTarget = true;

        StopAllCoroutines();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

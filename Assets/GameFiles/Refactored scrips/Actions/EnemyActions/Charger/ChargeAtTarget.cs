using System;
using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

[Serializable]
public class ChargeAtTarget : BaseEntityAction
{
    [SerializeField] private LayerMask bounceMask;
    [SerializeField] private int debrisSpawned = 6;
    [SerializeField] private float debrisDistance = 40f;
    [SerializeField] private float debrisSpeed = 25f;
    [SerializeField] private float chargeTime = 1f;
    [SerializeField] private float crashDownTime = 2.5f;


    private IUsesRigidBody usesRigidBody;
    private INavAgent navAgent;
    private ICrashCollider crashCollider;
    private IRadialProjectile radialProjectile;
    private Coroutine actionRoutine = null;

    private float hitInterval = 0.1f;
    private float timer = 0;

    public ChargeAtTarget() { }

    public ChargeAtTarget(bool preventsMovement, LayerMask bounceMask, int debrisSpawned, float debrisDistance, float debrisSpeed, float chargeTime, float crashDownTime)
    { 
        this.preventsMovement = preventsMovement;
        this.bounceMask = bounceMask;
        this.debrisSpawned = debrisSpawned;
        this.debrisSpeed = debrisSpeed;
        this.chargeTime = chargeTime;
        this.crashDownTime = crashDownTime;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is IUsesRigidBody usesRigidBody)) return;
        this.usesRigidBody = usesRigidBody;

        if (!(ownerEntity is INavAgent navAgent)) return;
        this.navAgent = navAgent;

        if (!(ownerEntity is ICrashCollider crashCollider)) { Debug.Log("owner entity is not of type ICrashCollider"); return; }
        this.crashCollider = crashCollider;

        if (!(ownerEntity is IRadialProjectile radialProjectile)) { Debug.Log("owner entity is not of type ICrashCollider"); return; }
        this.radialProjectile = radialProjectile;

        navAgent.DisableAIAgent();
        crashCollider.hasCrashed = false;
        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        Rigidbody rb = usesRigidBody.rb;

        yield return Vibrate(chargeTime, 0.025f);

        yield return ChargeTowardsTarget();
        yield return Crash();

        yield return new WaitForSeconds(crashDownTime);

        actionRoutine = null;
        EndAction();
    }

    private IEnumerator ChargeTowardsTarget()
    {
        Vector3 force = (ownerEntity.transform.forward).normalized;
        force.y = 0;
        force *= 50;

        while (!crashCollider.hasCrashed)
        { 
            usesRigidBody.rb.AddForce(force, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Vibrate(float duration, float intensity)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            ownerEntity.bodySystem.Vibrate(intensity);
            yield return null;
        }
    }

    private IEnumerator Crash()
    {
        yield return new WaitForSeconds(0.05f);
        SpawnDebris(debrisSpawned, 0);
    }

    private void SpawnDebris(int amount, float radius)
    {
        Vector3 reverseDirection = ownerEntity.transform.position - crashCollider.crashPosition;

        for (int i = 0; i < amount; i++)
        {
            float angle = ((180f / amount) * i) - 90;

            Vector3 projectileDirection = Quaternion.AngleAxis(angle, Vector3.up) * reverseDirection;
            Vector3 spawnPosition = crashCollider.crashPosition + (projectileDirection * radius);
            Quaternion projectileRotation = Quaternion.LookRotation(projectileDirection, Vector3.up);
            
            GameObject projectile = radialProjectile.radialObj;
            RadialProjectile rock = ObjectPoolManager.SpawnObject(projectile, spawnPosition, projectileRotation).GetComponent<RadialProjectile>();
            rock.Initialize(ownerEntity, debrisDistance, debrisSpeed);
        }
    }

    public override void UpdateAction()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            CheckForHitTarget();
            SetTimer();
        }
    }

    private void SetTimer()
    {
        timer = hitInterval;
    }

    private void CheckForHitTarget()
    {
        Vector3 position = ownerEntity.transform.position + ownerEntity.transform.forward * 5;
        Collider[] colliders = Physics.OverlapSphere(position, 3, ownerEntity.hostileMask);

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (collider.gameObject == ownerEntity) { continue; }

            collider.gameObject.GetComponent<Entity>().OnTakeDamage(5, Color.red, DamageType.Normal);
        }
    }

    public override void InterruptAction()
    {
        if (actionRoutine != null)
        {
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }

        EndAction();
    }

    public override void EndAction()
    {
        navAgent.EnableAIAgent();
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new ChargeAtTarget(preventsMovement, bounceMask, debrisSpawned, debrisDistance, debrisSpeed, chargeTime, crashDownTime);
    }
}

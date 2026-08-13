using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SlimeTrail : BaseEntityAction
{
    [SerializeField] float radius = 10f;
    [SerializeField] float frequency = 0.75f;
    [SerializeField] int initialDamage = 5;
    [SerializeField] int tickDamage = 2;
    [SerializeField] float duration = 10f;
    [SerializeField] float tickRate = 1f;

    private ISlimeTrail slimeTrail;
    private IGrounded grounded;
    private ISlimeSplit slimeSplit;
    private IUsesRigidBody usesRigidBody;
    private Coroutine actionRoutine = null;

    public SlimeTrail() { }

    public SlimeTrail(float radius, float frequency, int initialDamage, int tickDamage, float duration, float tickRate)
    {
        this.initialDamage = initialDamage;
        this.frequency = frequency;
        this.tickDamage = tickDamage;
        this.duration = duration;
        this.tickRate = tickRate;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is ISlimeTrail slimeTrail)) return;
        this.slimeTrail = slimeTrail;

        if (!(ownerEntity is IGrounded grounded)) return;
        this.grounded = grounded;

        if (!(ownerEntity is ISlimeSplit slimeSplit)) return;
        this.slimeSplit = slimeSplit;

        if (!(ownerEntity is IUsesRigidBody usesRigidBody)) return;
        this.usesRigidBody = usesRigidBody;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        yield return new WaitForSeconds(frequency);
        TrySpawnSlimeField();

        actionRoutine = null;
        EndAction();
    }

    private void TrySpawnSlimeField()
    {
        Ray ray = new Ray(ownerEntity.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, grounded.groundLayer))
        {
            FireField slimeField = ObjectPoolManager.SpawnObject(slimeTrail.slimeFieldObj, hit.point, Quaternion.identity).GetComponent<FireField>();
            slimeField.Initialize(ownerEntity, Color.pink, radius * slimeSplit.scale, initialDamage, tickDamage, duration, tickRate);
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
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new SlimeTrail(radius, frequency, initialDamage, tickDamage, duration, tickRate);
    }
}

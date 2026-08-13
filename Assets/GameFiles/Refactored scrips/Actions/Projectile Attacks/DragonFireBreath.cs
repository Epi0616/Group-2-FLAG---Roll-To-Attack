using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DragonFireBreath : BaseEntityAction
{
    [SerializeField] private Vector3 fireballSpawnOffset;
    [SerializeField] private int amount = 15;
    [SerializeField] private float delay = 0.07f;
    [SerializeField] private int initialDamage = 7;
    [SerializeField] private int tickDamage = 2;    

    private IFireballAction fireballAction;
    private IAnimated animated;
    private Coroutine actionRoutine;

    public DragonFireBreath() { }
    public DragonFireBreath(Vector3 fireballSpawnOffset, int amount, float delay, int initialDamage, int tickDamage)
    { 
        this.fireballSpawnOffset = fireballSpawnOffset;
        this.amount = amount;
        this.delay = delay;
        this.initialDamage = initialDamage;
        this.tickDamage = tickDamage;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is IFireballAction iFireballAction)) return;
        fireballAction = iFireballAction;

        if (!(ownerEntity is IAnimated animated)) return;
        this.animated = animated;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        //animated.animationManager.PlayAnimationCrossFade(AnimationType.Attack, 1, MixerType.complimentary, 0.2f, (amount * delay) + 1);

        for (int i = 0; i < amount; i++)
        {
            SpawnFireball();
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(1f);

        actionRoutine = null;
        EndAction();
    }

    private void SpawnFireball()
    {
        Transform rootBone = fireballAction.fireballRootBone.transform;
        Vector3 fireballPosition = rootBone.position + rootBone.rotation * fireballSpawnOffset;

        Vector3 inaccuracy = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f));
        Quaternion fireballRotation = Quaternion.LookRotation(ownerEntity.target.transform.position - fireballPosition + inaccuracy);

        Fireball fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, fireballPosition, fireballRotation).GetComponent<Fireball>();
        fireball.Initialize(ownerEntity, 50f, initialDamage, tickDamage);
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
        return new DragonFireBreath(fireballSpawnOffset, amount, delay, initialDamage, tickDamage);
    }
}

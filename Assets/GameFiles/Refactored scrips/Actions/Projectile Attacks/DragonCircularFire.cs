using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class DragonCircularFire : BaseEntityAction
{
    [SerializeField] private int amount = 10;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float distance = 75f;
    [SerializeField] private float speed = 10f;

    private IFireWingBeat fireWingBeat;
    private IAnimated animated;

    public DragonCircularFire() { }
    public DragonCircularFire(bool preventsMovement, int amount, float radius, float distance, float speed)
    {
        this.amount = amount;
        this.radius = radius;
        this.preventsMovement = preventsMovement;
        this.distance = distance;
        this.speed = speed;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        if (!(ownerEntity is IFireWingBeat iFireWingBeat)) return;

        fireWingBeat = iFireWingBeat;

        if (ownerEntity is IAnimated animated)
        {
            this.animated = animated;
        }

        ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        animated.animationManager.PlayAnimationCrossFade(AnimationType.Defend, 1, MixerType.main, 0.2f, 2.5f);

        yield return new WaitForSeconds(1.5f);
        SpawnCircleOfWingBeats(10, 5);

        yield return new WaitForSeconds(1.5f);

        EndAction();
    }

    private void SpawnCircleOfWingBeats(int amount, float radius)
    {
        for (int i = 0; i < amount; i++)
        {
            float angle = (360f / amount) * i;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 offset = rotation * Vector3.forward * radius;

            GameObject obj = fireWingBeat.fireWingBeatObj;
            Vector3 objPosition = ownerEntity.transform.position + offset + new Vector3 (0, -4.5f, 0);

            FireWingBeat wingBeat = ObjectPoolManager.SpawnObject(obj, objPosition, rotation).GetComponent<FireWingBeat>();
            wingBeat.Initialize(ownerEntity, 75f, 10f);
        }
    }

    public override void EndAction()
    {
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new DragonCircularFire(preventsMovement, amount, radius, distance, speed);
    }
}

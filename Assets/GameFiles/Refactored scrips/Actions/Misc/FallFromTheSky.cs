using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FallFromTheSky : BaseEntityAction
{
    public static event Action<float, Transform> BossFallingFromSky;

    [SerializeField] private int shieldStacks;
    [SerializeField] private float duration = 7.5f;

    private Vector3 endPosition;

    public FallFromTheSky() { }
    public FallFromTheSky(bool preventsMovement, int shieldStacks, float duration)
    {
        this.preventsMovement = preventsMovement;
        this.shieldStacks = shieldStacks;
        this.duration = duration;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        BossFallingFromSky?.Invoke(duration, ownerEntity.transform);

        ActiveStatusEffect shieldEffect = new(new ShieldedStatus(shieldStacks), new List<BaseCondition>() { new AlwaysTrueCondition(true) }, false);
        ownerEntity.statusSystem.OnRecieveEffect(shieldEffect);

        if (ownerEntity is IAnimated animated)
        {
            //animated.animationManager.PlayAnimationCrossFade(AnimationType.WakeUp, 1, MixerType.main, 0.2f, 5);
        }

        endPosition = ownerEntity.transform.position;
        endPosition.y -= 48f;// spawner specifies 50 in the air but model would put base plate in the floor

        ownerEntity.StartCoroutine(FallDown(ownerEntity.transform.position, endPosition, duration));
    }

    private IEnumerator FallDown(Vector3 start, Vector3 end, float duration)
    {
        float timer = duration;
        float t = 0;
        float easeInT = 0;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;
            easeInT = 1f - Mathf.Pow(Mathf.Max(0f, 1f - t), 0.9f);

            ownerEntity.transform.position = Vector3.Lerp(start, end, easeInT);
            yield return null;
        }

        EndAction();
    }

    public override void EndAction()
    {
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new FallFromTheSky(preventsMovement, shieldStacks, duration);
    }
}

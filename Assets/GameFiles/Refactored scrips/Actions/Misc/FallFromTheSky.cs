using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FallFromTheSky : BaseEntityAction
{
    public static event Action<float, Transform> BossFallingFromSky;

    [SerializeField] private int slamStacks;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float duration = 7.5f;

    private Vector3 startPosition;

    public FallFromTheSky() { }
    public FallFromTheSky(bool preventsMovement, int slamStacks, float damageMultiplier, float duration)
    {
        this.preventsMovement = preventsMovement;
        this.slamStacks = slamStacks;
        this.damageMultiplier = damageMultiplier;
        this.duration = duration;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        ownerEntity.bodySystem.SetVisibility(false);        

        ActiveStatusEffect shieldEffect = new(new SlamBlockedStatus(slamStacks, damageMultiplier), new List<BaseCondition>() { new AlwaysTrueCondition(true) }, false);
        ownerEntity.statusSystem.OnRecieveEffect(shieldEffect);

        if (ownerEntity is IAnimated animated)
        {
            //animated.animationManager.PlayAnimationCrossFade(AnimationType.WakeUp, 1, MixerType.main, 0.2f, 5);
        }

        startPosition = ownerEntity.transform.position;
        startPosition.y += 50f;

        ownerEntity.StartCoroutine(FallDown(startPosition, ownerEntity.transform.position, duration));
    }

    private IEnumerator FallDown(Vector3 start, Vector3 end, float duration)
    {
        float timer = duration;
        float t = 0;
        float easeInT = 0;

        BossFallingFromSky?.Invoke(duration, ownerEntity.transform);

        ownerEntity.transform.position = start;
        ownerEntity.bodySystem.SetVisibility(true);

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
        return new FallFromTheSky(preventsMovement, slamStacks, damageMultiplier, duration);
    }
}

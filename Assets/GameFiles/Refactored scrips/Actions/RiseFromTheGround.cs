using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RiseFromTheGround : BaseEntityAction
{
    private Vector3 endPosition;
    private float duration = 5f;

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.WakeUp, 1, MixerType.main, 0.2f, 5);
        }

        endPosition = ownerEntity.transform.position;
        endPosition.y += 9.5f;

        ownerEntity.StartCoroutine(RiseUp(ownerEntity.transform.position, endPosition, duration));
    }

    private IEnumerator RiseUp(Vector3 start, Vector3 end, float duration)
    { 
        float timer = duration;
        float t = 0;

        ownerEntity.bodySystem.HandleFixedVibrateTime(duration);
        while (t < 1)
        { 
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            ownerEntity.transform.position = Vector3.Lerp(start, end, t);
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
        return new RiseFromTheGround();
    }
}

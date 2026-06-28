using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Animation baseAnimation = new();
    private Animation currentAnimation = new();
    private Entity ownerEntity;

    private void Start()
    {
        SetBaseAction(EnemyAnimations.Waddle);
        currentAnimation = baseAnimation;
    }

    public void Initialize(Entity ownerEntity)
    { 
        this.ownerEntity = ownerEntity;
    }

    public void SetBaseAction(Animation newAnimation)
    { 
        baseAnimation.animationId = newAnimation.animationId;
        baseAnimation.priority = int.MaxValue;
    }

    public void PlayAnimation(Animation newAnimation, float crossFadeTime = 0.2f)
    {
        if (currentAnimation.animationId != newAnimation.animationId)
        {
            if (newAnimation.priority <= currentAnimation.priority)
            {
                Debug.Log("has priority");
                currentAnimation = newAnimation;
                animator.CrossFade(newAnimation.animationId, crossFadeTime);
            }
        }
    }

    public void PlayAnimationWithDelay(Animation newAnimation, float delay, float crossFadeTime = 0.2f)
    {
        StartCoroutine(DelayAnimation(newAnimation, delay, crossFadeTime));
    }

    private IEnumerator DelayAnimation(Animation newAnimation, float delay, float crossFadeTime = 0.2f)
    { 
        yield return new WaitForSeconds(delay);
        PlayAnimation(newAnimation, crossFadeTime);
    }

    public void HandleAnimationOver(float time, float crossFadeTime)
    {
        if (ownerEntity.healthSystem.isDead) return;
        currentAnimation = new Animation(0, int.MaxValue);
        StartCoroutine(DelayBaseAnimationStart(time, crossFadeTime));
    }

    private IEnumerator DelayBaseAnimationStart(float timer, float crossFadeTime)
    {
        yield return new WaitForSeconds(timer);

        PlayAnimation(baseAnimation);
    }
}

public struct Animation
{
    public int animationId;
    public int priority;

    public Animation(int animationId, int priority)
    {
        this.animationId = animationId;
        this.priority = priority;
    }
}

public static class EnemyAnimations
{
    public static readonly Animation WakeUp = new Animation(Animator.StringToHash("WakeUp"), 1);
    public static readonly Animation Waddle = new Animation(Animator.StringToHash("Waddle"), 1);
    public static readonly Animation Attack = new Animation(Animator.StringToHash("Attack"), 1);
    public static readonly Animation Death = new Animation(Animator.StringToHash("Death"), 0);
}

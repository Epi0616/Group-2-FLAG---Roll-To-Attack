using UnityEngine;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SwitchAnimation(int newAnimation, float crossFadeTime = 0.2f)
    {
        animator.CrossFade(newAnimation, crossFadeTime);
    }
}

public static class EnemyAnimations
{
    public static readonly int WakeUp = Animator.StringToHash("WakeUp");
    public static readonly int Waddle = Animator.StringToHash("Waddle");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Death = Animator.StringToHash("Death");
}

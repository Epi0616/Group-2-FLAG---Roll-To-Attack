using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClipType[] animationClipTypes;

    private Entity ownerEntity;

    private PlayableGraph graph;
    private AnimationMixerPlayable mixer;
    private AnimationPlayableOutput output;

    private AnimationType currentType;
    private int currentPriority;
    private Dictionary<AnimationType, AnimationClip> animationClips;
    private Dictionary<AnimationType, AnimationClipPlayable> activatedClips;

    private void OnEnable()
    {
        SetupPlayableGraph();
        UnpackAnimationClips();
        currentType = AnimationType.Idle;
        currentPriority = int.MaxValue;
    }

    private void OnDisable()
    {
        graph.Destroy();
    }

    public void Initialize(Entity entity)
    {
        ownerEntity = entity;
    }

    private void SetupPlayableGraph()
    {
        graph = PlayableGraph.Create("GolemAnimationGraph");
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        mixer = AnimationMixerPlayable.Create(graph, 2);
        output = AnimationPlayableOutput.Create(graph, "Animation", animator);

        output.SetSourcePlayable(mixer);
        graph.Play();
    }

    private void UnpackAnimationClips()
    {
        activatedClips = new Dictionary<AnimationType, AnimationClipPlayable>();
        animationClips = new Dictionary<AnimationType, AnimationClip>();

        foreach (AnimationClipType animationClipType in animationClipTypes)
        { 
            AnimationClip animation = animationClipType.clip;
            AnimationClipPlayable playableAnimation = AnimationClipPlayable.Create(graph, animation);

            animationClips.Add(animationClipType.type, animation);
            activatedClips.Add(animationClipType.type, playableAnimation);
        }
    }

    public void PlayAnimation(AnimationType animationType, int priority, float window = 0f)
    {

        if (CheckForCanBePlayed(priority, animationType))
        {
            if (GetPlayableAnimationFromType(animationType, out AnimationClipPlayable newAnimation))
            {
                if (mixer.GetInput(0).IsValid())
                {
                    graph.Disconnect(mixer, 0);
                }

                if (GetAnimationClipFromType(animationType, out AnimationClip animationClip))
                {
                    if (window == 0)
                    {
                        newAnimation.SetSpeed(1);
                    }
                    else
                    {
                        newAnimation.SetSpeed(animationClip.length / window);
                    }
                }

                graph.Connect(newAnimation, 0, mixer, 0);

                animator.speed = 1;
                newAnimation.SetTime(0);
                mixer.SetInputWeight(0, 1);

                currentType = animationType;
                currentPriority = priority;
            }
        }
    }

    public void PlayAnimationCrossFade(AnimationType animationType, int priority, float crossFadeDuration = 0.2f, float window = 0f)
    {
        if (CheckForCanBePlayed(priority, animationType))
        {
            StartCoroutine(CrossFade(animationType, priority, crossFadeDuration, window));
        }
    }

    private IEnumerator CrossFade(AnimationType animationType, int priority, float crossFadeDuration, float window)
    {
        if (GetPlayableAnimationFromType(animationType, out AnimationClipPlayable newAnimation))
        {
            if (GetPlayableAnimationFromType(currentType, out AnimationClipPlayable currentPlayable))
            {

                if (mixer.GetInput(0).IsValid())
                {
                    graph.Disconnect(mixer, 0);
                }
                if (mixer.GetInput(1).IsValid())
                {
                    graph.Disconnect(mixer, 1);
                }

                if (GetAnimationClipFromType(animationType, out AnimationClip animationClip))
                {
                    if (window == 0)
                    {
                        newAnimation.SetSpeed(1);
                    }
                    else
                    {
                        newAnimation.SetSpeed(animationClip.length / window);
                    }
                }

                graph.Connect(currentPlayable, 0, mixer, 0);
                graph.Connect(newAnimation, 0, mixer, 1);

                newAnimation.SetTime(0);

                float timer = crossFadeDuration;
                float t = 0f;

                while (t < 1)
                {
                    timer -= Time.deltaTime;
                    t = (crossFadeDuration - timer) / crossFadeDuration;

                    mixer.SetInputWeight(0, 1f - t);
                    mixer.SetInputWeight(1, t);
                    yield return null;
                }

                currentType = animationType;
                currentPriority = priority;
                //currentClip = newAnimation;
            }
            else // if there is no valid curent action crossfade wont work so pass over to regular play to double check it cant be played - after testing i dont think this point ever gets reached...
            {
                PlayAnimation(animationType, priority);
            }
        }
    }


    private bool CheckForCanBePlayed(int priority, AnimationType animationType)
    {
        if (GetPlayableAnimationFromType(currentType, out AnimationClipPlayable playableClip))
        {
            if (IsAnimationFinished(playableClip))
            {
                if (GetAnimationClipFromType(currentType, out AnimationClip animationClip))
                {
                    if (!animationClip.isLooping)
                    {
                        currentType = AnimationType.None;
                        currentPriority = int.MaxValue;
                    }
                }
            }
        }

        if (animationType == currentType) return false;
        if (priority > currentPriority) return false;
        return true;
    }

    public bool GetAnimationClipFromType(AnimationType clipType, out AnimationClip outClip)
    {
        if (animationClips.TryGetValue(clipType, out AnimationClip clip))
        {
            outClip = clip;
            return true;
        }
        outClip = null;
        return false;
    }
    private bool GetPlayableAnimationFromType(AnimationType clipType, out AnimationClipPlayable outClip)
    {
        if (activatedClips.TryGetValue(clipType, out AnimationClipPlayable playableClip))
        {
            outClip = playableClip;
            return true;
        }
        outClip = default;
        return false;
    }

    private bool IsAnimationFinished(AnimationClipPlayable animation)
    {
        return animation.GetTime() >= animation.GetDuration();
    }
}

[Serializable]
public struct AnimationClipType
{
    public AnimationType type;
    public AnimationClip clip;
    public AnimationClipType(AnimationType type, AnimationClip clip)
    {
        this.type = type;
        this.clip = clip;
    }
}

public enum AnimationType
{
    None,
    Idle,
    WakeUp,
    Waddle,
    Attack,
    Charge,
    RockThrow,
    Death
}

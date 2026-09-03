using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class AnimationOnDemandManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClipType[] animationClipTypes;

    private Entity ownerEntity;

    private PlayableGraph graph;
    private AnimationPlayableOutput mainAnimationOutput, complimentaryAnimationOutput;

    private Dictionary<AnimationType, AnimationClip> animationClips;

    AnimationMixerContainer mainMixer, complimentaryMixer;
    private int currentPriority;

    public bool graphActive = false;

    private void OnEnable()
    {
        SetupPlayableGraph();
        currentPriority = int.MaxValue;
    }

    private void OnDisable()
    {
        graphActive = false;
        EndCurrentAnimation(MixerType.main);
        EndCurrentAnimation(MixerType.complimentary);
        graph.Destroy();
    }

    public void Initialize()
    {
        UnpackAnimationClipTypes();
        if (animationClips.Count <= 0)
        {
            graphActive = false;
        }
    }

    private void UnpackAnimationClipTypes()
    {
        animationClips = new Dictionary<AnimationType, AnimationClip>();

        foreach (AnimationClipType animationClipType in animationClipTypes)
        {
            AnimationClip animation = animationClipType.clip;
            animationClips.Add(animationClipType.type, animation);
        }
    }

    private void SetupPlayableGraph()
    {
        graph = PlayableGraph.Create("AnimationGraph");
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        mainMixer = new AnimationMixerContainer(
            AnimationMixerPlayable.Create(graph, 2),
            MixerType.main,
            AnimationType.None,
            int.MaxValue,
            0
        );

        complimentaryMixer = new AnimationMixerContainer(
            AnimationMixerPlayable.Create(graph, 2),
            MixerType.complimentary,
            AnimationType.None,
            int.MaxValue,
            0
        );

        mainAnimationOutput = AnimationPlayableOutput.Create(graph, "mainAnimation", animator);
        complimentaryAnimationOutput = AnimationPlayableOutput.Create(graph, "complimentaryAnimation", animator);

        animator.speed = 1;

        mainAnimationOutput.SetSourcePlayable(mainMixer.mixer);
        complimentaryAnimationOutput.SetSourcePlayable(complimentaryMixer.mixer);
        graph.Play();
        graphActive = true;
    }

    public void EndCurrentAnimation(MixerType mixerType)
    {
        if (!graphActive) return;
        if (!GetMixerContainerFromType(mixerType, out AnimationMixerContainer mixer)) return;
        CancelCurrentCrossFade(mixer);
        DestroyMixerPlayable(mixer, 0);

        mixer.animationType = AnimationType.None;
        mixer.priority = int.MaxValue;
    }

    public void PauseCurrentAnimation(MixerType mixerType)
    {
        if (!graphActive) return;
        if (!GetMixerContainerFromType(mixerType, out AnimationMixerContainer mixer)) return;

        Playable tempPlayable = mixer.mixer.GetInput(0);
        if (!tempPlayable.IsValid()) return;

        tempPlayable.SetSpeed(0);
    }

    public void ResumeCurrentAnimation(MixerType mixerType)
    {
        if (!graphActive) return;
        if (!GetMixerContainerFromType(mixerType, out AnimationMixerContainer mixer)) return;

        Playable tempPlayable = mixer.mixer.GetInput(0);
        if (!tempPlayable.IsValid()) return;

        tempPlayable.SetSpeed(mixer.currentAnimationSpeed);
    }

    public void PlayAnimation(AnimationType newAnimationType, int priority, MixerType mixerType, float window = 0f)
    {
        if (!graphActive) return;
        if (!GetMixerContainerFromType(mixerType, out AnimationMixerContainer mixer)) return;
        if (!CheckForCanBePlayed(mixer, priority, newAnimationType)) return;
        if (!GetAnimationClipFromType(newAnimationType, out AnimationClip animationClip)) return;

        AnimationClipPlayable newPlayable = CreatePlayableFromClip(animationClip);

        CancelCurrentCrossFade(mixer);
        DestroyMixerPlayable(mixer, 0);

        SetPlayableSpeed(mixer, ref newPlayable, window);
        ConnectPlayable(newPlayable, mixer);

        mixer.animationType = newAnimationType;
        mixer.priority = priority;
    }

    public void PlayAnimationCrossFade(AnimationType newAnimationType, int priority, MixerType mixerType, float crossFadeDuration = 0.2f, float window = 0f)
    {
        if (!graphActive) return;
        if (!GetMixerContainerFromType(mixerType, out AnimationMixerContainer mixer)) return;
        if (!CheckForCanBePlayed(mixer, priority, newAnimationType)) return;
        if (!GetAnimationClipFromType(newAnimationType, out AnimationClip animationClip)) return;

        CancelCurrentCrossFade(mixer);

        AnimationClipPlayable newPlayable = CreatePlayableFromClip(animationClip);
        Playable currentPlayable = mixer.mixer.GetInput(0);
        mixer.mixer.DisconnectInput(0);

        SetPlayableSpeed(mixer, ref newPlayable, window);
        mixer.crossFadeRoutine = StartCoroutine(ConnectPlayableCrossFade(newPlayable, currentPlayable, mixer, crossFadeDuration));

        mixer.priority = priority;
        mixer.animationType = newAnimationType;
    }

    private void ConnectPlayable(AnimationClipPlayable newPlayable, AnimationMixerContainer mixer)
    {
        graph.Connect(newPlayable, 0, mixer.mixer, 0);

        newPlayable.SetTime(0);
        mixer.mixer.SetInputWeight(0, 1);
    }

    private IEnumerator ConnectPlayableCrossFade(AnimationClipPlayable newPlayable, Playable currentPlayable, AnimationMixerContainer mixer, float crossFadeDuration)
    {
        graph.Connect(newPlayable, 0, mixer.mixer, 0);
        graph.Connect(currentPlayable, 0, mixer.mixer, 1);

        newPlayable.SetTime(0);

        float timer = crossFadeDuration;
        float t = 0f;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (crossFadeDuration - timer) / crossFadeDuration;

            mixer.mixer.SetInputWeight(0, t);
            mixer.mixer.SetInputWeight(1, 1f - t);
            yield return null;
        }

        mixer.mixer.SetInputWeight(0, 1);
        DestroyMixerPlayable(mixer, 1);
    }

    private void SetPlayableSpeed(AnimationMixerContainer mixer, ref AnimationClipPlayable newPlayable, float window)
    {
        float speed = 1;
        if (window != 0)
        {
            speed = newPlayable.GetAnimationClip().length / window;
        }

        newPlayable.SetSpeed(speed);
        mixer.currentAnimationSpeed = speed;
    }

    private void CancelCurrentCrossFade(AnimationMixerContainer mixer)
    {
        if (mixer.crossFadeRoutine != null)
        {
            StopCoroutine(mixer.crossFadeRoutine);
            DestroyMixerPlayable(mixer, 1);
            mixer.mixer.SetInputWeight(0, 1);
        }
    }

    private void DestroyMixerPlayable(AnimationMixerContainer mixer, int index)
    {
        if (mixer.mixer.GetInput(index).IsValid())
        {
            Playable temp = mixer.mixer.GetInput(index);
            graph.Disconnect(mixer.mixer, index);
            temp.Destroy();
        }
    }

    private bool CheckForCanBePlayed(AnimationMixerContainer mixer, int priority, AnimationType animationType)
    {
        //Debug.Log($"checking for type {animationType}");

        if (IsCurrentAnimationFinished(mixer))
        {
            if (GetAnimationClipFromType(mixer.animationType, out AnimationClip animationClip))
            {
                if (!animationClip.isLooping)
                {
                    mixer.animationType = AnimationType.None;
                    currentPriority = int.MaxValue;
                }
            }
        }

        if (animationType == mixer.animationType) return false;
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

    public bool GetMixerContainerFromType(MixerType mixerType, out AnimationMixerContainer outMixer)
    {
        switch (mixerType)
        {
            case MixerType.main:
                outMixer = mainMixer;
                return true;
            case MixerType.complimentary:
                outMixer = complimentaryMixer;
                return true;
            default: 
                outMixer = default;
                return false;
        }
    }

    private AnimationClipPlayable CreatePlayableFromClip(AnimationClip animationClip)
    {
        return AnimationClipPlayable.Create(graph, animationClip);
    }

    private bool IsCurrentAnimationFinished(AnimationMixerContainer mixer)
    {
        if (mixer == null) return false;
        //if (!mixer.mixer.IsValid()) return false;

        AnimationClipPlayable currentPlayable = (AnimationClipPlayable)mixer.mixer.GetInput(0);
        if (!currentPlayable.IsValid()) return true;
        return currentPlayable.GetTime() >= currentPlayable.GetAnimationClip().length;
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

public class AnimationMixerContainer
{
    public AnimationMixerPlayable mixer;

    public Coroutine crossFadeRoutine;
    public MixerType mixerType;
    public AnimationType animationType;
    public int priority;
    public float currentAnimationSpeed;

    public AnimationMixerContainer(AnimationMixerPlayable mixer, MixerType mixerType, AnimationType animationType, int priority, float currentAnimationSpeed)
    {
        this.mixer = mixer;
        this.mixerType = mixerType;
        this.animationType = animationType;
        this.priority = priority;
        this.currentAnimationSpeed = currentAnimationSpeed;
    }
}

public enum MixerType
{ 
    main,
    complimentary
}

public enum AnimationType
{
    None,
    Idle,
    WakeUp,
    Waddle,
    Attack,
    Defend,
    DefendCharge,
    Charge,
    RockThrow,
    OnStunned,
    Stunned,
    StunnedOver,
    Scream,
    ScreamUpwards,
    Death
}
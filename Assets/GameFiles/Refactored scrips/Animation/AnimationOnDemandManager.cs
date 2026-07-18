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
    //private AnimationMixerPlayable mainMixer, complimentaryMixer;
    private AnimationPlayableOutput mainAnimationOutput, complimentaryAnimationOutput;

    private Dictionary<AnimationType, AnimationClip> animationClips;

    private Coroutine crossFadeAnimation;
    MixerContainer mainMixer, complimentaryMixer;
    private int currentPriority;

    private void OnEnable()
    {
        SetupPlayableGraph();
        currentPriority = int.MaxValue;
    }

    private void OnDisable()
    {
        graph.Destroy();
    }

    public void Initialize(Entity entity)
    {
        ownerEntity = entity;
        UnpackAnimationClipTypes();
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

        mainMixer = new MixerContainer(
            AnimationMixerPlayable.Create(graph, 2),
            MixerType.main,
            default,
            int.MaxValue
        );

        complimentaryMixer = new MixerContainer(
            AnimationMixerPlayable.Create(graph, 2),
            MixerType.main,
            default,
            int.MaxValue
        );

        mainAnimationOutput = AnimationPlayableOutput.Create(graph, "mainAnimation", animator);
        complimentaryAnimationOutput = AnimationPlayableOutput.Create(graph, "complimentaryAnimation", animator);

        animator.speed = 1;

        mainAnimationOutput.SetSourcePlayable(mainMixer.mixer);
        complimentaryAnimationOutput.SetSourcePlayable(complimentaryMixer.mixer);
        graph.Play();
    }

    public void PlayAnimation(AnimationType newAnimationType, int priority, MixerType mixerType, float window = 0f)
    {
        if (!GetMixerContainerFromType(mixerType, out MixerContainer mixer)) return;
        if (!CheckForCanBePlayed(mixer, priority, newAnimationType)) return;
        if (!GetAnimationClipFromType(newAnimationType, out AnimationClip animationClip)) return;

        AnimationClipPlayable newPlayable = CreatePlayableFromClip(animationClip);

        CancelCurrentCrossFade(mixer);
        DestroyMixerPlayable(mixer, 0);

        SetPlayableSpeed(ref newPlayable, window);
        ConnectPlayable(newPlayable, mixer);

        mixer.animationType = newAnimationType;
        mixer.priority = priority;
    }

    public void PlayAnimationCrossFade(AnimationType newAnimationType, int priority, MixerType mixerType, float crossFadeDuration = 0.2f, float window = 0f)
    {
        if (!GetMixerContainerFromType(mixerType, out MixerContainer mixer)) return;
        if (!CheckForCanBePlayed(mixer, priority, newAnimationType)) return;
        if (!GetAnimationClipFromType(newAnimationType, out AnimationClip animationClip)) return;

        CancelCurrentCrossFade(mixer);

        AnimationClipPlayable newPlayable = CreatePlayableFromClip(animationClip);
        Playable currentPlayable = mixer.mixer.GetInput(0);
        mixer.mixer.DisconnectInput(0);

        SetPlayableSpeed(ref newPlayable, window);
        crossFadeAnimation = StartCoroutine(ConnectPlayableCrossFade(newPlayable, currentPlayable, mixer, crossFadeDuration));

        mixer.priority = priority;
        mixer.animationType = newAnimationType;
    }

    private void ConnectPlayable(AnimationClipPlayable newPlayable, MixerContainer mixer)
    {
        graph.Connect(newPlayable, 0, mixer.mixer, 0);

        newPlayable.SetTime(0);
        mixer.mixer.SetInputWeight(0, 1);
    }

    private IEnumerator ConnectPlayableCrossFade(AnimationClipPlayable newPlayable, Playable currentPlayable, MixerContainer mixer, float crossFadeDuration)
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

        DestroyMixerPlayable(mixer, 1);
    }

    private void SetPlayableSpeed(ref AnimationClipPlayable newPlayable, float window)
    {
        if (window == 0)
        {
            newPlayable.SetSpeed(1);
        }
        else
        {
            newPlayable.SetSpeed(newPlayable.GetAnimationClip().length / window);
        }
    }

    private void CancelCurrentCrossFade(MixerContainer mixer)
    {
        if (crossFadeAnimation != null)
        {
            StopCoroutine(crossFadeAnimation);
            DestroyMixerPlayable(mixer, 1);
        }
    }

    private void DestroyMixerPlayable(MixerContainer mixer, int index)
    {
        if (mixer.mixer.GetInput(index).IsValid())
        {
            Playable temp = mixer.mixer.GetInput(index);
            graph.Disconnect(mixer.mixer, index);
            temp.Destroy();
        }
    }

    private bool CheckForCanBePlayed(MixerContainer mixer, int priority, AnimationType animationType)
    {
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

    public bool GetMixerContainerFromType(MixerType mixerType, out MixerContainer outMixer)
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

    private bool IsCurrentAnimationFinished(MixerContainer mixer)
    {
        AnimationClipPlayable currentPlayable = (AnimationClipPlayable)mixer.mixer.GetInput(0);
        if (!currentPlayable.IsValid()) return true;
        return currentPlayable.GetTime() >= currentPlayable.GetAnimationClip().length;
    }
}

public struct MixerContainer
{
    public AnimationMixerPlayable mixer;

    public MixerType mixerType;
    public AnimationType animationType;
    public int priority;

    public MixerContainer(AnimationMixerPlayable mixer, MixerType mixerType, AnimationType animationType, int priority)
    { 
        this.mixer = mixer;
        this.mixerType = mixerType;
        this.animationType = animationType;
        this.priority = priority;
    }
}

public enum MixerType
{ 
    main,
    complimentary
}

